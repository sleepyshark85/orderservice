namespace Pixelz.OrderStore;

public class CheckoutFailedEvent : CheckoutEvent
{
    public const string Type = "CheckoutFailed";
    public string FailureReason { get; set; }

    public override string EventType => Type;
}