using System;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.CircuitBreaker;

namespace Pixelz.OrderStore;

public interface IPaymentAdapter
{
    Task<PaymentResult> ProcessPaymentAsync(Guid orderId);
}

public class DefaultPaymentAdapter : IPaymentAdapter
{
    private readonly IPaymentServiceClient _client;
    private readonly ILogger<DefaultPaymentAdapter> _logger;
    private readonly AsyncCircuitBreakerPolicy _asyncCircuitBreakerPolicy;

    public DefaultPaymentAdapter(IPaymentServiceClient client, ILogger<DefaultPaymentAdapter> logger)
    {
        _client = client;
        _logger = logger;

        _asyncCircuitBreakerPolicy = Policy.Handle<Exception>()
            .CircuitBreakerAsync(
                exceptionsAllowedBeforeBreaking: 5,
                durationOfBreak: TimeSpan.FromMinutes(1)
            );
    }

    public async Task<PaymentResult> ProcessPaymentAsync(Guid orderId)
    {
        try
        {
            return await _asyncCircuitBreakerPolicy.ExecuteAsync(async () =>
            {
                var request = new PaymentServiceRequest
                {
                    OrderId = orderId
                };

                var response = await _client.ProcessPaymentAsync(request);

                return new PaymentResult
                {
                    Success = response.Success,
                    TransactionId = response.TransactionId,
                    ErrorCode = response.ErrorCode,
                    ErrorMessage = response.ErrorMessage,
                    Timestamp = DateTime.UtcNow
                };
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error processing payment for order {OrderId}", orderId);

            return new PaymentResult
            {
                Success = false,
                ErrorCode = "SYSTEM_ERROR",
                ErrorMessage = "An unexpected error occurred while processing payment",
                Timestamp = DateTime.UtcNow
            };
        }
    }
}

public class PaymentResult
{ 
    public bool Success { get; set; }
    public string TransactionId { get; set; }
    public DateTime Timestamp { get; set; }
    public string ErrorCode { get; set; }
    public string ErrorMessage { get; set; }
}