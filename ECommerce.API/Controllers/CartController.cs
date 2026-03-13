using ECommerce.API.Common.Extensions;
using ECommerce.API.DTOs.Cart;
using ECommerce.API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[ApiController]
[Route("api/cart")]
[Authorize]
public class CartController : ControllerBase
{
    private readonly ICartService _cartService;

    public CartController(ICartService cartService)
    {
        _cartService = cartService;
    }

    /// <summary>
    /// Get current user's cart items.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(List<CartItemResponse>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCart()
    {
        var userId = User.GetUserId();
        var items = await _cartService.GetCartAsync(userId);
        return Ok(items);
    }

    /// <summary>
    /// Add a product to cart.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(CartItemResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddToCart([FromBody] CartItemRequest request)
    {
        var userId = User.GetUserId();
        var item = await _cartService.AddToCartAsync(userId, request);
        return Ok(item);
    }

    /// <summary>
    /// Remove a product from cart by product ID.
    /// </summary>
    [HttpDelete("{productId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveFromCart(int productId)
    {
        var userId = User.GetUserId();
        await _cartService.RemoveFromCartAsync(userId, productId);
        return NoContent();
    }
}
