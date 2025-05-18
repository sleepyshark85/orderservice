namespace Pixelz.OrderStore;

public class CheckoutInitiatedEvent : CheckoutEvent
{
    public const string Type = "CheckoutInitiated";
    public string InitiatedBy { get; set; }

    public override string EventType => Type;
}