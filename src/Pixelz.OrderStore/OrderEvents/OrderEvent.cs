namespace Pixelz.OrderStore;

public abstract class OrderEvent : PixelzEvent
{
    public override string EventId => GetEventId(OrderId);
    public Guid OrderId { get; set; }
    public DateTime Timestamp { get; private set; }

    protected OrderEvent()
    {
        Timestamp = DateTime.UtcNow;
    }

    public static string GetEventId(Guid orderId) => $"order-{orderId}";
}