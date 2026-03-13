using ECommerce.API.Common.Exceptions;
using ECommerce.API.Data;
using ECommerce.API.DTOs.Cart;
using ECommerce.API.Entities;
using ECommerce.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Services;

public class CartService : ICartService
{
    private readonly AppDbContext _context;

    public CartService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<CartItemResponse>> GetCartAsync(int userId)
    {
        return await _context.CartItems
            .Where(ci => ci.UserId == userId)
            .Include(ci => ci.Product)
            .Select(ci => new CartItemResponse
            {
                Id = ci.Id,
                ProductId = ci.ProductId,
                ProductName = ci.Product.Name,
                ProductPrice = ci.Product.Price,
                ImageUrl = ci.Product.ImageUrl,
                Quantity = ci.Quantity
            })
            .ToListAsync();
    }

    public async Task<CartItemResponse> AddToCartAsync(int userId, CartItemRequest request)
    {
        var product = await _context.Products.FindAsync(request.ProductId)
            ?? throw new NotFoundException("Product not found.");

        if (product.StockQuantity < request.Quantity)
            throw new BadRequestException($"Not enough stock. Available: {product.StockQuantity}.");

        var existingItem = await _context.CartItems
            .FirstOrDefaultAsync(ci => ci.UserId == userId && ci.ProductId == request.ProductId);

        if (existingItem != null)
        {
            var newQuantity = existingItem.Quantity + request.Quantity;
            if (product.StockQuantity < newQuantity)
                throw new BadRequestException($"Not enough stock. Available: {product.StockQuantity}, in cart: {existingItem.Quantity}.");

            existingItem.Quantity = newQuantity;
        }
        else
        {
            existingItem = new CartItem
            {
                UserId = userId,
                ProductId = request.ProductId,
                Quantity = request.Quantity
            };
            _context.CartItems.Add(existingItem);
        }

        await _context.SaveChangesAsync();

        return new CartItemResponse
        {
            Id = existingItem.Id,
            ProductId = product.Id,
            ProductName = product.Name,
            ProductPrice = product.Price,
            ImageUrl = product.ImageUrl,
            Quantity = existingItem.Quantity
        };
    }

    public async Task RemoveFromCartAsync(int userId, int productId)
    {
        var cartItem = await _context.CartItems
            .FirstOrDefaultAsync(ci => ci.UserId == userId && ci.ProductId == productId)
            ?? throw new NotFoundException("Cart item not found.");

        _context.CartItems.Remove(cartItem);
        await _context.SaveChangesAsync();
    }
}
