using ECommerce.API.Common.Exceptions;
using ECommerce.API.Data;
using ECommerce.API.DTOs.Order;
using ECommerce.API.Entities;
using ECommerce.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Services;

public class OrderService : IOrderService
{
    private readonly AppDbContext _context;

    public OrderService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<OrderResponse> CheckoutAsync(int userId)
    {
        var cartItems = await _context.CartItems
            .Where(ci => ci.UserId == userId)
            .Include(ci => ci.Product)
            .ToListAsync();

        if (cartItems.Count == 0)
            throw new BadRequestException("Cart is empty.");

        // Validate stock for all items
        foreach (var item in cartItems)
        {
            if (item.Product.StockQuantity < item.Quantity)
                throw new BadRequestException(
                    $"Not enough stock for '{item.Product.Name}'. Available: {item.Product.StockQuantity}, requested: {item.Quantity}.");
        }

        var order = new Order
        {
            UserId = userId,
            OrderDate = DateTime.UtcNow,
            Status = "Pending",
            TotalAmount = cartItems.Sum(ci => ci.Product.Price * ci.Quantity),
            OrderItems = cartItems.Select(ci => new OrderItem
            {
                ProductId = ci.ProductId,
                Quantity = ci.Quantity,
                PriceAtPurchase = ci.Product.Price
            }).ToList()
        };

        // Decrease stock
        foreach (var item in cartItems)
        {
            item.Product.StockQuantity -= item.Quantity;
        }

        _context.Orders.Add(order);
        _context.CartItems.RemoveRange(cartItems);
        await _context.SaveChangesAsync();

        return await GetOrderResponseAsync(order.Id);
    }

    public async Task<List<OrderResponse>> GetUserOrdersAsync(int userId)
    {
        return await _context.Orders
            .Where(o => o.UserId == userId)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .OrderByDescending(o => o.OrderDate)
            .Select(o => new OrderResponse
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                Status = o.Status,
                TotalAmount = o.TotalAmount,
                Items = o.OrderItems.Select(oi => new OrderItemResponse
                {
                    ProductId = oi.ProductId,
                    ProductName = oi.Product.Name,
                    Quantity = oi.Quantity,
                    PriceAtPurchase = oi.PriceAtPurchase
                }).ToList()
            })
            .ToListAsync();
    }

    public async Task<OrderResponse> GetOrderByIdAsync(int userId, int orderId)
    {
        var order = await _context.Orders
            .Where(o => o.Id == orderId && o.UserId == userId)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync()
            ?? throw new NotFoundException("Order not found.");

        return new OrderResponse
        {
            Id = order.Id,
            OrderDate = order.OrderDate,
            Status = order.Status,
            TotalAmount = order.TotalAmount,
            Items = order.OrderItems.Select(oi => new OrderItemResponse
            {
                ProductId = oi.ProductId,
                ProductName = oi.Product.Name,
                Quantity = oi.Quantity,
                PriceAtPurchase = oi.PriceAtPurchase
            }).ToList()
        };
    }

    public async Task<List<OrderResponse>> GetAllOrdersAsync()
    {
        return await _context.Orders
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .OrderByDescending(o => o.OrderDate)
            .Select(o => new OrderResponse
            {
                Id = o.Id,
                OrderDate = o.OrderDate,
                Status = o.Status,
                TotalAmount = o.TotalAmount,
                Items = o.OrderItems.Select(oi => new OrderItemResponse
                {
                    ProductId = oi.ProductId,
                    ProductName = oi.Product.Name,
                    Quantity = oi.Quantity,
                    PriceAtPurchase = oi.PriceAtPurchase
                }).ToList()
            })
            .ToListAsync();
    }

    private async Task<OrderResponse> GetOrderResponseAsync(int orderId)
    {
        var order = await _context.Orders
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .FirstAsync(o => o.Id == orderId);

        return new OrderResponse
        {
            Id = order.Id,
            OrderDate = order.OrderDate,
            Status = order.Status,
            TotalAmount = order.TotalAmount,
            Items = order.OrderItems.Select(oi => new OrderItemResponse
            {
                ProductId = oi.ProductId,
                ProductName = oi.Product.Name,
                Quantity = oi.Quantity,
                PriceAtPurchase = oi.PriceAtPurchase
            }).ToList()
        };
    }
}
