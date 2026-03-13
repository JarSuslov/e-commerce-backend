namespace ECommerce.API.Entities;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Role { get; set; } = "Customer";

    public ICollection<CartItem> CartItems { get; set; } = [];
    public ICollection<Order> Orders { get; set; } = [];
}
