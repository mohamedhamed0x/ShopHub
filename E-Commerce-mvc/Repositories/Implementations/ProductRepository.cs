using E_Commerce_mvc.Data;
using E_Commerce_mvc.Models.Entities;
using E_Commerce_mvc.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_mvc.Repositories.Implementations
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
            => await _context.Products.Include(p => p.Category).ToListAsync();

        public async Task<IEnumerable<Product>> GetActiveAsync()
            => await _context.Products
                .Include(p => p.Category)
                .Where(p => p.IsActive)
                .ToListAsync();

        public async Task<(IEnumerable<Product> Products, int TotalCount)> GetFilteredAsync(
            int? categoryId, string? searchTerm, string? sortBy, int page, int pageSize)
        {
            var query = _context.Products
                .Include(p => p.Category)
                .Where(p => p.IsActive)
                .AsQueryable();

            // Filter by category
            if (categoryId.HasValue && categoryId > 0)
                query = query.Where(p => p.CategoryId == categoryId.Value);

            // Search by name or description
            if (!string.IsNullOrWhiteSpace(searchTerm))
                query = query.Where(p =>
                    p.Name.Contains(searchTerm) ||
                    (p.Description != null && p.Description.Contains(searchTerm)));

            // Get total count before pagination
            var totalCount = await query.CountAsync();

            // Sort
            query = sortBy?.ToLower() switch
            {
                "price_asc" => query.OrderBy(p => p.DiscountPrice ?? p.Price),
                "price_desc" => query.OrderByDescending(p => p.DiscountPrice ?? p.Price),
                "name" => query.OrderBy(p => p.Name),
                "newest" => query.OrderByDescending(p => p.CreatedAt),
                _ => query.OrderByDescending(p => p.CreatedAt)
            };

            // Paginate
            var products = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (products, totalCount);
        }

        public async Task<Product?> GetByIdAsync(int id)
            => await _context.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.Id == id);

        public async Task<Product?> GetByIdWithDetailsAsync(int id)
            => await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

        public async Task AddAsync(Product product)
            => await _context.Products.AddAsync(product);

        public void Update(Product product)
            => _context.Products.Update(product);

        public void Delete(Product product)
            => _context.Products.Remove(product);

        public async Task<bool> ExistsAsync(int id)
            => await _context.Products.AnyAsync(p => p.Id == id);

        public async Task<int> GetCountAsync()
            => await _context.Products.CountAsync(p => p.IsActive);

        public async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();
    }
}
