using ECommerce.API.DTOs.Cart;

namespace ECommerce.API.Services.Interfaces;

public interface ICartService
{
    Task<List<CartItemResponse>> GetCartAsync(int userId);
    Task<CartItemResponse> AddToCartAsync(int userId, CartItemRequest request);
    Task RemoveFromCartAsync(int userId, int productId);
}
