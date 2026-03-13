using E_Commerce_mvc.Data;
using E_Commerce_mvc.Models.Entities;
using E_Commerce_mvc.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_mvc.Repositories.Implementations
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
            => await _context.Categories.ToListAsync();

        public async Task<IEnumerable<Category>> GetActiveAsync()
            => await _context.Categories.Where(c => c.IsActive).ToListAsync();

        public async Task<Category?> GetByIdAsync(int id)
            => await _context.Categories.FindAsync(id);

        public async Task<Category?> GetByIdWithProductsAsync(int id)
            => await _context.Categories
                .Include(c => c.Products.Where(p => p.IsActive))
                .FirstOrDefaultAsync(c => c.Id == id);

        public async Task AddAsync(Category category)
            => await _context.Categories.AddAsync(category);

        public void Update(Category category)
            => _context.Categories.Update(category);

        public void Delete(Category category)
            => _context.Categories.Remove(category);

        public async Task<bool> ExistsAsync(int id)
            => await _context.Categories.AnyAsync(c => c.Id == id);

        public async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();
    }
}
