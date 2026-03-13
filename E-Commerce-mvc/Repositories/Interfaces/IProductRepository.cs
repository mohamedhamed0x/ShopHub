using E_Commerce_mvc.Models.Entities;

namespace E_Commerce_mvc.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product>> GetAllAsync();
        Task<IEnumerable<Product>> GetActiveAsync();
        Task<(IEnumerable<Product> Products, int TotalCount)> GetFilteredAsync(
            int? categoryId, string? searchTerm, string? sortBy, int page, int pageSize);
        Task<Product?> GetByIdAsync(int id);
        Task<Product?> GetByIdWithDetailsAsync(int id);
        Task AddAsync(Product product);
        void Update(Product product);
        void Delete(Product product);
        Task<bool> ExistsAsync(int id);
        Task<int> GetCountAsync();
        Task SaveChangesAsync();
    }
}
