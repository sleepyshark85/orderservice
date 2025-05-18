using Microsoft.AspNetCore.Mvc;
using Pixelz.OrderStore;

namespace Pixelz.OrderService.Controllers;

[ApiController]
[Route("api/v1/orders")]
public class CheckoutController : ControllerBase
{
    private readonly ILogger<CheckoutController> _logger;
    private readonly ICheckoutSaga _checkoutSaga;

    public CheckoutController(ILogger<CheckoutController> logger, ICheckoutSaga checkoutSaga)
    {
        _logger = logger;
        _checkoutSaga = checkoutSaga;
    }

    [HttpPost("{id:guid}/checkout")]
    public async Task<IActionResult> CheckoutOrder(Guid id, [FromBody] CheckoutOrderRequest request)
    {
        await _checkoutSaga.ExecuteCheckoutAsync(id, request.CheckoutBy);

        return Ok();
    }
}

public class CheckoutOrderRequest
{
    public required string CheckoutBy { get; set; }
}