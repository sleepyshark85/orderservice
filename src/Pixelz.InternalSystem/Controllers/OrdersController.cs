using Microsoft.AspNetCore.Mvc;
using Pixelz.OrderStore;

namespace Pixelz.InternalSystem.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly ILogger<OrdersController> _logger;
    private readonly IEventStoreRepository _eventStoreRepository;

    public OrdersController(ILogger<OrdersController> logger, IEventStoreRepository eventStoreRepository)
    {
        _logger = logger;
        _eventStoreRepository = eventStoreRepository;
    }

    //For the simplification/demostration purpose, I define create order endpoint here. 
    // But based on the requirement, the internal system will take care of the order status. It probably means orders should be created else where.
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
    {
        var orderEvent = new OrderCreatedEvent()
        {
            OrderId = Guid.NewGuid(),
            Name = request.Name,
            ClientId = request.ClientId,
            Items = request.Items.Select(e => new OrderItem()
            {
                Description = e.Description,
                Quantity = e.Quantity,
                UnitPrice = e.UnitPrice
            }).ToList()
        };

        var eventId = await _eventStoreRepository.SaveAsync(orderEvent);

        return Ok(eventId);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetOrder(Guid id)
    {
        var order = await _eventStoreRepository.GetOrderAsync(id);
        return Ok(order);
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders([FromQuery] string? name)
    {
        throw new NotImplementedException();
    }
}

//I don't really familiar with e-Commerce so I don't really know what kind of information we usually have. 
//I defined a very simple set of information here. That would be enough to demonstrate the service.
public class CreateOrderRequest
{
    public required string Name { get; set; }
    public required string ClientId { get; set; }
    public required List<OrderItemRequest> Items { get; set; }
}

public class OrderItemRequest
{
    public required string Description { get; set; }
    public required int Quantity { get; set; }
    public required decimal UnitPrice { get; set; }
}