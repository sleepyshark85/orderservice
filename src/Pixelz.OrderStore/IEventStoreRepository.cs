namespace Pixelz.OrderStore;

public interface IEventStoreRepository
{
    Task<string> SaveAsync<T>(T @event) where T : PixelzEvent;
    Task<Order?> GetOrderAsync(Guid orderId);
    Task<Checkout> GetCheckoutAsync(Guid checkoutId);
}