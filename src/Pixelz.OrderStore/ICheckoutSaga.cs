using Microsoft.Extensions.Logging;

namespace Pixelz.OrderStore;

public interface ICheckoutSaga
{
    Task<CheckoutResult> ExecuteCheckoutAsync(Guid orderId, string checkoutBy);
}

public class DefaultCheckoutSaga : ICheckoutSaga
{
    private readonly ILogger<DefaultCheckoutSaga> _logger;
    private readonly IPaymentAdapter _paymentAdapter;
    private readonly IEventStoreRepository _eventStoreRepository;

    public DefaultCheckoutSaga(ILogger<DefaultCheckoutSaga> logger, IPaymentAdapter paymentAdapter, IEventStoreRepository eventStoreRepository)
    {
        _logger = logger;
        _paymentAdapter = paymentAdapter;
        _eventStoreRepository = eventStoreRepository;
    }

    public async Task<CheckoutResult> ExecuteCheckoutAsync(Guid orderId, string checkoutBy)
    {
        _logger.LogInformation("Starting checkout process for order {OrderId}", orderId);

        var checkoutId = Guid.NewGuid();

        try
        {
            // Get order details
            var order = await _eventStoreRepository.GetOrderAsync(orderId);
            if (order == null)
            {
                _logger.LogWarning("Order {OrderId} not found", orderId);
                return CheckoutResult.Failed("Order not found");
            }

            // Validate order can be checked out
            if (order.Status != OrderStatus.CREATED)
            {
                _logger.LogWarning("Order {OrderId} has invalid status for checkout: {Status}", orderId, order.Status);
                return CheckoutResult.Failed($"Order has invalid status: {order.Status}");
            }

            var checkoutInitiatedEvent = new CheckoutInitiatedEvent
            {
                OrderId = orderId,
                CheckoutId = checkoutId,
                InitiatedBy =checkoutBy
            };

            // Persist initial state
            await _eventStoreRepository.SaveAsync(checkoutInitiatedEvent);

            var orderCheckoutInitiatedEvent = new OrderStatusChanged()
            {
                OrderId = orderId,
                Status = OrderStatus.CHECKOUT_INITIATED
            };

            // Update order status
            await _eventStoreRepository.SaveAsync(orderCheckoutInitiatedEvent);

            // Start first step
            return await ProcessPayment(checkoutId, orderId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error initiating checkout saga for order {OrderId}", orderId);

            // Persist failed state
            try
            {
                var checkoutFailedEvent = new CheckoutFailedEvent()
                {
                    OrderId = orderId,
                    CheckoutId = checkoutId,
                    FailureReason = ex.Message
                };

                await _eventStoreRepository.SaveAsync(checkoutFailedEvent);
            }
            catch (Exception innerEx)
            {
                _logger.LogError(innerEx, "Error persisting failed checkout {CheckoutId} state for order {OrderId}", checkoutId, orderId);
            }

            return CheckoutResult.Failed($"Error initiating checkout: {ex.Message}");
        }
    }

    private async Task<CheckoutResult> ProcessPayment(Guid checkoutId, Guid orderId)
    {
        _logger.LogInformation("Processing payment for order {OrderId} with checkout {CheckoutId}", orderId, checkoutId);

        // Send payment command
        var paymentResult = await _paymentAdapter.ProcessPaymentAsync(orderId);

        if (!paymentResult.Success)
        {
            _logger.LogWarning("Payment failed for order {OrderId}: {ErrorCode} - {ErrorMessage}",
                orderId, paymentResult.ErrorCode, paymentResult.ErrorMessage);

            var checkoutFailedEvent = new CheckoutFailedEvent()
            {
                OrderId = orderId,
                CheckoutId = checkoutId,
                FailureReason = $"{paymentResult.ErrorCode} - {paymentResult.ErrorMessage}"
            };

            await _eventStoreRepository.SaveAsync(checkoutFailedEvent);

            var orderCheckoutFailedEvent = new OrderStatusChanged()
            {
                OrderId = orderId,
                Status = OrderStatus.PAYMENT_FAILED
            };

            // Update order status
            await _eventStoreRepository.SaveAsync(orderCheckoutFailedEvent);

            // Return failed result
            return CheckoutResult.Failed($"Payment failed: {paymentResult.ErrorCode} - {paymentResult.ErrorMessage}");
        }

        var checkoutPaidEvent = new CheckoutPaidEvent()
        {
            OrderId = orderId,
            CheckoutId = checkoutId,
            TransactionId = paymentResult.TransactionId
        };

        await _eventStoreRepository.SaveAsync(checkoutPaidEvent);

        var orderCheckoutPaidEvent = new OrderStatusChanged()
        {
            OrderId = orderId,
            Status = OrderStatus.PAID
        };

        // Update order status
        await _eventStoreRepository.SaveAsync(orderCheckoutPaidEvent);
        
        return CheckoutResult.Successful(checkoutId);
    }
}

public class CheckoutResult
{
    public Guid? CheckoutId { get; private set; }
    public bool Success { get; private set; }
    public bool PartialSuccess { get; private set; }
    public string Message { get; private set; }
    public string ErrorCode { get; private set; }

    public static CheckoutResult Successful(Guid checkoutId)
    {
        return new CheckoutResult
        {
            CheckoutId = checkoutId,
            Success = true,
            PartialSuccess = false,
            Message = "Checkout completed successfully"
        };
    }

    public static CheckoutResult PartiallySuccessful(Guid checkoutId, string message)
    {
        return new CheckoutResult
        {
            CheckoutId = checkoutId,
            Success = false,
            PartialSuccess = true,
            Message = message
        };
    }

    public static CheckoutResult Failed(string message, Guid? checkoutId = null, string errorCode = null)
    {
        return new CheckoutResult
        {
            CheckoutId = checkoutId,
            Success = false,
            PartialSuccess = false,
            Message = message,
            ErrorCode = errorCode
        };
    }
}