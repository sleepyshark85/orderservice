using Microsoft.AspNetCore.Mvc;

namespace Pixelz.OrderService.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(ILogger<OrdersController> logger)
    {
        _logger = logger;
    }

    //For the simplification/demostration purpose, I define create order endpoint here. 
    // But based on the requirement, the internal system will take care of the order status. It probably means orders should be created else where.
    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request)
    {
        throw new NotImplementedException();
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetOrder(Guid id)
    {
        throw new NotImplementedException();
    }

    [HttpGet]
    public async Task<IActionResult> GetOrders([FromQuery] string? name)
    {
        throw new NotImplementedException();
    }

    [HttpPost("{id:guid}/checkout")]
    public async Task<IActionResult> CheckoutOrder(Guid id, [FromBody] CheckoutOrderRequest request)
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

public class CheckoutOrderRequest
{
    public required string CheckoutBy { get; set; }
    public required string PaymentMethod { get; set; }
}