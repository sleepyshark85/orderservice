using System.Text.Json;

namespace Pixelz.OrderStore;

public class OrderCreatedEvent : OrderEvent
{
    public const string Type = "OrderCreated";
    
    public override string EventType => Type;
    public string Name { get; set; } = string.Empty;
    public string ClientId { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public List<OrderItem> Items { get; set; } = new();
}

public class OrderItem
{
    public string Description { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}