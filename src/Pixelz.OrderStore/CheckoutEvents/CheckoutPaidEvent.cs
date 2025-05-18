namespace Pixelz.OrderStore;

public class CheckoutPaidEvent : CheckoutEvent
{
    public const string Type = "CheckoutPaid";
    public string TransactionId { get; set; }

    public override string EventType => Type;
}