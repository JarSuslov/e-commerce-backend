using ECommerce.API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (await context.Users.AnyAsync())
            return;

        var passwordHasher = new PasswordHasher<User>();

        // Seed Admin user
        var admin = new User
        {
            Email = "admin@store.com",
            Name = "Admin",
            Role = "Admin"
        };
        admin.PasswordHash = passwordHasher.HashPassword(admin, "Admin123!");

        // Seed Customer user
        var customer = new User
        {
            Email = "customer@store.com",
            Name = "John Doe",
            Role = "Customer"
        };
        customer.PasswordHash = passwordHasher.HashPassword(customer, "Customer123!");

        context.Users.AddRange(admin, customer);
        await context.SaveChangesAsync();

        // Seed Categories
        var electronics = new Category { Name = "Electronics", Description = "Electronic devices and gadgets" };
        var clothing = new Category { Name = "Clothing", Description = "Men's and women's clothing" };
        var books = new Category { Name = "Books", Description = "Fiction and non-fiction books" };

        context.Categories.AddRange(electronics, clothing, books);
        await context.SaveChangesAsync();

        // Seed Products
        var products = new List<Product>
        {
            new()
            {
                Name = "Wireless Headphones",
                Description = "Bluetooth over-ear headphones with noise cancellation",
                Price = 79.99m,
                StockQuantity = 50,
                CategoryId = electronics.Id,
                ImageUrl = "https://example.com/images/headphones.jpg"
            },
            new()
            {
                Name = "Smartphone X200",
                Description = "Latest flagship smartphone with 6.7 inch display",
                Price = 999.99m,
                StockQuantity = 30,
                CategoryId = electronics.Id,
                ImageUrl = "https://example.com/images/smartphone.jpg"
            },
            new()
            {
                Name = "USB-C Hub",
                Description = "7-in-1 USB-C hub with HDMI and Ethernet",
                Price = 34.99m,
                StockQuantity = 100,
                CategoryId = electronics.Id,
                ImageUrl = "https://example.com/images/usbhub.jpg"
            },
            new()
            {
                Name = "Cotton T-Shirt",
                Description = "Premium 100% cotton unisex t-shirt",
                Price = 24.99m,
                StockQuantity = 200,
                CategoryId = clothing.Id,
                ImageUrl = "https://example.com/images/tshirt.jpg"
            },
            new()
            {
                Name = "Denim Jeans",
                Description = "Classic straight-fit denim jeans",
                Price = 59.99m,
                StockQuantity = 80,
                CategoryId = clothing.Id,
                ImageUrl = "https://example.com/images/jeans.jpg"
            },
            new()
            {
                Name = "Clean Code",
                Description = "A Handbook of Agile Software Craftsmanship by Robert C. Martin",
                Price = 39.99m,
                StockQuantity = 60,
                CategoryId = books.Id,
                ImageUrl = "https://example.com/images/cleancode.jpg"
            }
        };

        context.Products.AddRange(products);
        await context.SaveChangesAsync();
    }
}
