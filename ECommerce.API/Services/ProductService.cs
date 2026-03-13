using ECommerce.API.Common.Exceptions;
using ECommerce.API.Data;
using ECommerce.API.DTOs.Product;
using ECommerce.API.Entities;
using ECommerce.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Services;

public class ProductService : IProductService
{
    private readonly AppDbContext _context;

    public ProductService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResponse<ProductResponse>> GetAllAsync(ProductQueryParams queryParams)
    {
        var query = _context.Products.Include(p => p.Category).AsQueryable();

        if (!string.IsNullOrWhiteSpace(queryParams.Search))
            query = query.Where(p => p.Name.ToLower().Contains(queryParams.Search.ToLower()));

        if (queryParams.CategoryId.HasValue)
            query = query.Where(p => p.CategoryId == queryParams.CategoryId.Value);

        var totalCount = await query.CountAsync();

        var page = Math.Max(1, queryParams.Page);
        var pageSize = Math.Clamp(queryParams.PageSize, 1, 50);

        var items = await query
            .OrderBy(p => p.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new ProductResponse
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                CategoryId = p.CategoryId,
                CategoryName = p.Category.Name,
                ImageUrl = p.ImageUrl
            })
            .ToListAsync();

        return new PagedResponse<ProductResponse>
        {
            Items = items,
            TotalCount = totalCount,
            Page = page,
            PageSize = pageSize
        };
    }

    public async Task<ProductResponse> GetByIdAsync(int id)
    {
        var product = await _context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id)
            ?? throw new NotFoundException("Product not found.");

        return new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            CategoryId = product.CategoryId,
            CategoryName = product.Category.Name,
            ImageUrl = product.ImageUrl
        };
    }

    public async Task<ProductResponse> CreateAsync(ProductRequest request)
    {
        if (!await _context.Categories.AnyAsync(c => c.Id == request.CategoryId))
            throw new NotFoundException("Category not found.");

        var product = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            StockQuantity = request.StockQuantity,
            CategoryId = request.CategoryId,
            ImageUrl = request.ImageUrl
        };

        _context.Products.Add(product);
        await _context.SaveChangesAsync();

        var category = await _context.Categories.FindAsync(product.CategoryId);

        return new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            CategoryId = product.CategoryId,
            CategoryName = category!.Name,
            ImageUrl = product.ImageUrl
        };
    }

    public async Task<ProductResponse> UpdateAsync(int id, ProductRequest request)
    {
        var product = await _context.Products.FindAsync(id)
            ?? throw new NotFoundException("Product not found.");

        if (!await _context.Categories.AnyAsync(c => c.Id == request.CategoryId))
            throw new NotFoundException("Category not found.");

        product.Name = request.Name;
        product.Description = request.Description;
        product.Price = request.Price;
        product.StockQuantity = request.StockQuantity;
        product.CategoryId = request.CategoryId;
        product.ImageUrl = request.ImageUrl;

        await _context.SaveChangesAsync();

        var category = await _context.Categories.FindAsync(product.CategoryId);

        return new ProductResponse
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            StockQuantity = product.StockQuantity,
            CategoryId = product.CategoryId,
            CategoryName = category!.Name,
            ImageUrl = product.ImageUrl
        };
    }

    public async Task DeleteAsync(int id)
    {
        var product = await _context.Products.FindAsync(id)
            ?? throw new NotFoundException("Product not found.");

        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }
}
