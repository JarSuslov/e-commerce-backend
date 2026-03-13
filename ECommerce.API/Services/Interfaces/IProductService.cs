using ECommerce.API.DTOs.Product;

namespace ECommerce.API.Services.Interfaces;

public interface IProductService
{
    Task<PagedResponse<ProductResponse>> GetAllAsync(ProductQueryParams queryParams);
    Task<ProductResponse> GetByIdAsync(int id);
    Task<ProductResponse> CreateAsync(ProductRequest request);
    Task<ProductResponse> UpdateAsync(int id, ProductRequest request);
    Task DeleteAsync(int id);
}
