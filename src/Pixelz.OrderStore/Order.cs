namespace Pixelz.OrderStore;

public class Order
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string ClientId { get; private set; } = string.Empty;
    public decimal Amount { get; private set; }
    public OrderStatus Status { get; set; }
    public List<OrderItemModel> Items { get; private set; } = new();
    public bool IsCheckedOut { get; private set; }
    public string? CheckoutBy { get; private set; }
    public DateTime? CheckoutDate { get; private set; }
    public string? PaymentMethod { get; private set; }
    public string? InvoiceNumber { get; private set; }

    private Order() { }

    public static Order Create(OrderCreatedEvent @event)
    {
        var order = new Order
        {
            Id = @event.OrderId,
            Name = @event.Name,
            Status = OrderStatus.CREATED,
            ClientId = @event.ClientId,
            Amount = @event.Amount,
            Items = @event.Items.Select(i => new OrderItemModel
            {
                Description = i.Description,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList()
        };
        return order;
    }

    // public void ApplyCheckout(OrderCheckedOutEvent @event)
    // {
    //     IsCheckedOut = true;
    //     CheckoutBy = @event.CheckoutBy;
    //     CheckoutDate = @event.CheckoutDate;
    //     PaymentMethod = @event.PaymentMethod;
    //     InvoiceNumber = @event.InvoiceNumber;
    // }
}

public enum OrderStatus
{
    CREATED,
    CHECKOUT_INITIATED,
    PAYMENT_FAILED,
    PAID,
    INVOICE_CREATED,
    EMAIL_SENT,
    PRODUCTION_SUBMITED,
    COMPLETED
}

public class OrderItemModel
{
    public string Description { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice => Quantity * UnitPrice;
}