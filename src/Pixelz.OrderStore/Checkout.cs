namespace Pixelz.OrderStore;

public class Checkout
{
    public Guid Id { get; private set; }

    public Checkout(Guid id)
    {
        Id = id;
    }
}
