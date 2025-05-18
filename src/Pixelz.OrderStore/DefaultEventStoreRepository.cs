using System.Text;
using System.Text.Json;
using EventStore.Client;

namespace Pixelz.OrderStore;

public class DefaultEventStoreRepository : IEventStoreRepository
{
    private readonly EventStoreClient _eventStoreClient;

    public DefaultEventStoreRepository(EventStoreClient eventStoreClient)
    {
        _eventStoreClient = eventStoreClient;
    }

    public async Task<string> SaveAsync<T>(T @event) where T : PixelzEvent
    {
        var eventData = new EventData(
            Uuid.NewUuid(),
            @event.EventType,
            Encoding.UTF8.GetBytes(JsonSerializer.Serialize(@event)),
            metadata: null);

        await _eventStoreClient.AppendToStreamAsync(
            @event.EventId,
            StreamState.Any,
            [eventData]);

        return @event.EventId;
    }

    public async Task<Order?> GetOrderAsync(Guid orderId)
    {
        var streamName = OrderEvent.GetEventId(orderId);
        var result = _eventStoreClient.ReadStreamAsync(
            Direction.Forwards,
            streamName,
            StreamPosition.Start);

        if (await result.ReadState == ReadState.StreamNotFound)
        {
            return null;
        }

        Order? order = null;

        await foreach (var @event in result)
        {
            if (@event.Event.EventType == OrderCreatedEvent.Type)
            {
                var createdEvent = JsonSerializer.Deserialize<OrderCreatedEvent>(@event.Event.Data.ToArray());
                if (createdEvent != null)
                {
                    order = Order.Create(createdEvent);
                }
            }
            // else if (@event.Event.EventType == OrderCheckedOutEvent.Type && order != null)
            // {
            //     var checkedOutEvent = JsonSerializer.Deserialize<OrderCheckedOutEvent>(@event.Event.Data.ToArray());
            //     if (checkedOutEvent != null)
            //     {
            //         order.ApplyCheckout(checkedOutEvent);
            //     }
            // }
        }

        return order;
    }

    public Task<Checkout> GetCheckoutAsync(Guid checkoutId)
    {
        throw new NotImplementedException();
    }
}