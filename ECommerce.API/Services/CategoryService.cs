using ECommerce.API.Common.Exceptions;
using ECommerce.API.Data;
using ECommerce.API.DTOs.Category;
using ECommerce.API.Entities;
using ECommerce.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.API.Services;

public class CategoryService : ICategoryService
{
    private readonly AppDbContext _context;

    public CategoryService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<CategoryResponse>> GetAllAsync()
    {
        return await _context.Categories
            .Select(c => new CategoryResponse
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description
            })
            .ToListAsync();
    }

    public async Task<CategoryResponse> GetByIdAsync(int id)
    {
        var category = await _context.Categories.FindAsync(id)
            ?? throw new NotFoundException("Category not found.");

        return new CategoryResponse
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description
        };
    }

    public async Task<CategoryResponse> CreateAsync(CategoryRequest request)
    {
        if (await _context.Categories.AnyAsync(c => c.Name == request.Name))
            throw new ConflictException("Category with this name already exists.");

        var category = new Category
        {
            Name = request.Name,
            Description = request.Description
        };

        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        return new CategoryResponse
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description
        };
    }

    public async Task<CategoryResponse> UpdateAsync(int id, CategoryRequest request)
    {
        var category = await _context.Categories.FindAsync(id)
            ?? throw new NotFoundException("Category not found.");

        if (await _context.Categories.AnyAsync(c => c.Name == request.Name && c.Id != id))
            throw new ConflictException("Category with this name already exists.");

        category.Name = request.Name;
        category.Description = request.Description;
        await _context.SaveChangesAsync();

        return new CategoryResponse
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description
        };
    }

    public async Task DeleteAsync(int id)
    {
        var category = await _context.Categories
            .Include(c => c.Products)
            .FirstOrDefaultAsync(c => c.Id == id)
            ?? throw new NotFoundException("Category not found.");

        if (category.Products.Count != 0)
            throw new BadRequestException("Cannot delete category that has products.");

        _context.Categories.Remove(category);
        await _context.SaveChangesAsync();
    }
}
