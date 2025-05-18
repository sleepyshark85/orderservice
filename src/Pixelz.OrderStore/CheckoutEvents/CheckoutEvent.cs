using System.Text.Json;

namespace Pixelz.OrderStore;

public abstract class CheckoutEvent : PixelzEvent
{
    public override string EventId => GetEventId(CheckoutId, OrderId);
    public Guid OrderId { get; set; }
    public Guid CheckoutId { get; set; }
    public DateTime Timestamp { get; private set; }

    protected CheckoutEvent()
    {
        Timestamp = DateTime.UtcNow;
    }

    public static string GetEventId(Guid checkoutId, Guid orderId) => $"checkout-{orderId}-{checkoutId}";
}