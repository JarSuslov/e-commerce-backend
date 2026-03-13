using ECommerce.API.Common.Extensions;
using ECommerce.API.DTOs.Order;
using ECommerce.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[ApiController]
[Route("api/orders")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    /// <summary>
    /// Checkout — create order from current cart.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Checkout()
    {
        var userId = User.GetUserId();
        var order = await _orderService.CheckoutAsync(userId);
        return CreatedAtAction(nameof(GetById), new { id = order.Id }, order);
    }

    /// <summary>
    /// Get current user's orders.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<OrderResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetMyOrders()
    {
        var userId = User.GetUserId();
        var orders = await _orderService.GetUserOrdersAsync(userId);
        return Ok(orders);
    }

    /// <summary>
    /// Get a specific order by ID (only own orders).
    /// </summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(OrderResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(int id)
    {
        var userId = User.GetUserId();
        var order = await _orderService.GetOrderByIdAsync(userId, id);
        return Ok(order);
    }
}
