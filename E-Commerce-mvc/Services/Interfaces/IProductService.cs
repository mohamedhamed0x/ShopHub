using E_Commerce_mvc.Models.Entities;

namespace E_Commerce_mvc.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<(IEnumerable<Product> Products, int TotalCount)> GetFilteredProductsAsync(
            int? categoryId, string? searchTerm, string? sortBy, int page, int pageSize = 9);
        Task<Product?> GetProductByIdAsync(int id);
        Task CreateProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task ToggleProductStatusAsync(int id);
        Task<int> GetProductCountAsync();
    }
}
