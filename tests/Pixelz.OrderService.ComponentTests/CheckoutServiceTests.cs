using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Pixelz.OrderService.Controllers;

namespace Pixelz.OrderService.ComponentTests;

[Collection("API Tests")]
public class CheckoutServiceTests : IClassFixture<CheckoutServiceFactory>
{
    private readonly HttpClient _client;
    private readonly CheckoutServiceFactory _factory;

    public CheckoutServiceTests(CheckoutServiceFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    // [Fact]
    // public async Task Get_Orders_Without_Specify_Name_Results_In_All_Orders()
    // {
    //     // Arrange

    //     // Act
    //     var response = await _client.GetAsync("/api/v1/orders");
        
    //     // Assert
    //     response.StatusCode.Should().Be(HttpStatusCode.OK);
    // }

    // [Theory]
    // [InlineData("apple")]
    // public async Task Get_Orders_With_A_Specific_Name_Results_In_All_Orders_Has_Name_Contains_The_Specified_Name(string name)
    // {
    //     // Arrange

    //     // Act
    //     var response = await _client.GetAsync($"/api/v1/orders?name={name}");

    //     // Assert
    //     response.StatusCode.Should().Be(HttpStatusCode.OK);
    // }

    // [Fact]
    // public async Task Create_A_Valid_Order_Results_In_A_Valid_Response()
    // {
    //     // Arrange
    //     var newOrderId = Guid.NewGuid();

    //     var request = new CreateOrderRequest
    //     {
    //         Name = "New Test Order",
    //         ClientId = "client789",
    //         Items = new List<OrderItemRequest>
    //         {
    //             new() { Description = "Item 1", Quantity = 2, UnitPrice = 10 },
    //             new() { Description = "Item 2", Quantity = 1, UnitPrice = 20 }
    //         }
    //     };
        
    //     // Act
    //     var response = await _client.PostAsJsonAsync("/api/v1/orders", request);

    //     // Assert
    //     response.StatusCode.Should().Be(HttpStatusCode.Created);
    // }

    [Fact]
    public async Task Checkout_A_Valid_Order_Results_In_A_Valid_Response()
    {
        // Arrange
        var orderId = Guid.NewGuid();

        var request = new CheckoutOrderRequest
        {
            CheckoutBy = "test@example.com",
            PaymentMethod = "CreditCard"
        };
        
        // Act
        var response = await _client.PostAsJsonAsync($"/api/v1/orders/{orderId}/checkout", request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}