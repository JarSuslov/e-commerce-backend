using ECommerce.API.DTOs.Category;

namespace ECommerce.API.Services.Interfaces;

public interface ICategoryService
{
    Task<List<CategoryResponse>> GetAllAsync();
    Task<CategoryResponse> GetByIdAsync(int id);
    Task<CategoryResponse> CreateAsync(CategoryRequest request);
    Task<CategoryResponse> UpdateAsync(int id, CategoryRequest request);
    Task DeleteAsync(int id);
}
