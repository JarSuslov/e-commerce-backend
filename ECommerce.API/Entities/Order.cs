namespace ECommerce.API.Entities;

public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime OrderDate { get; set; }
    public string Status { get; set; } = "Pending";
    public decimal TotalAmount { get; set; }

    public User User { get; set; } = null!;
    public ICollection<OrderItem> OrderItems { get; set; } = [];
}
