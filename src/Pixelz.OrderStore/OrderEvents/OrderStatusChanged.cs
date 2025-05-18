namespace Pixelz.OrderStore;

public class OrderStatusChanged : OrderEvent
{
    public const string Type = "OrderStatusChanged";
    public override string EventType => Type;
    public OrderStatus Status { get; set; }
}