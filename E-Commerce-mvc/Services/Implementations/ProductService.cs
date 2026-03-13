using E_Commerce_mvc.Models.Entities;
using E_Commerce_mvc.Repositories.Interfaces;
using E_Commerce_mvc.Services.Interfaces;

namespace E_Commerce_mvc.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
            => await _productRepository.GetAllAsync();

        public async Task<(IEnumerable<Product> Products, int TotalCount)> GetFilteredProductsAsync(
            int? categoryId, string? searchTerm, string? sortBy, int page, int pageSize = 9)
            => await _productRepository.GetFilteredAsync(categoryId, searchTerm, sortBy, page, pageSize);

        public async Task<Product?> GetProductByIdAsync(int id)
            => await _productRepository.GetByIdAsync(id);

        public async Task CreateProductAsync(Product product)
        {
            product.CreatedAt = DateTime.UtcNow;
            await _productRepository.AddAsync(product);
            await _productRepository.SaveChangesAsync();
        }

        public async Task UpdateProductAsync(Product product)
        {
            _productRepository.Update(product);
            await _productRepository.SaveChangesAsync();
        }

        public async Task ToggleProductStatusAsync(int id)
        {
            var product = await _productRepository.GetByIdAsync(id);
            if (product != null)
            {
                product.IsActive = !product.IsActive;
                _productRepository.Update(product);
                await _productRepository.SaveChangesAsync();
            }
        }

        public async Task<int> GetProductCountAsync()
            => await _productRepository.GetCountAsync();
    }
}
