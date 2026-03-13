namespace ECommerce.API.DTOs.Cart;

public class CartItemResponse
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal ProductPrice { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal Subtotal => ProductPrice * Quantity;
}
