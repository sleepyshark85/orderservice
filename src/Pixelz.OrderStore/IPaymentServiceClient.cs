using System;

namespace Pixelz.OrderStore;

public interface IPaymentServiceClient
{
    Task<PaymentServiceResponse> ProcessPaymentAsync(PaymentServiceRequest request);
}

public class DefaultPaymentServiceClient : IPaymentServiceClient
{
    public Task<PaymentServiceResponse> ProcessPaymentAsync(PaymentServiceRequest request)
    {
        return Task.FromResult(PaymentServiceResponse.Successful());
    }
}

public class PaymentServiceRequest
{
    public Guid OrderId { get; set; }
}

public class PaymentServiceResponse
{
    public bool Success { get; private set; }
    public string TransactionId { get; private set; }
    public DateTime Timestamp { get; private set; }
    public string ErrorCode { get; private set; }
    public string ErrorMessage { get; private set; }
    
    public static PaymentServiceResponse Successful()
    {
        return new PaymentServiceResponse()
        {
            Success = true,
            TransactionId = Guid.NewGuid().ToString(),
            Timestamp = DateTime.UtcNow
        };
    }
}