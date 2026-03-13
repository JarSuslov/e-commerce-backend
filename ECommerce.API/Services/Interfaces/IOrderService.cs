using ECommerce.API.DTOs.Order;

namespace ECommerce.API.Services.Interfaces;

public interface IOrderService
{
    Task<OrderResponse> CheckoutAsync(int userId);
    Task<List<OrderResponse>> GetUserOrdersAsync(int userId);
    Task<OrderResponse> GetOrderByIdAsync(int userId, int orderId);
    Task<List<OrderResponse>> GetAllOrdersAsync();
}
