namespace ECommerce.API.DTOs.Order;

public class OrderItemResponse
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal PriceAtPurchase { get; set; }
    public decimal Subtotal => PriceAtPurchase * Quantity;
}
