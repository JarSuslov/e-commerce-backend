namespace ECommerce.API.DTOs.Order;

public class OrderResponse
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public List<OrderItemResponse> Items { get; set; } = [];
}
