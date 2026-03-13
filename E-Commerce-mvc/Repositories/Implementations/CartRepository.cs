using E_Commerce_mvc.Data;
using E_Commerce_mvc.Models.Entities;
using E_Commerce_mvc.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_mvc.Repositories.Implementations
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _context;

        public CartRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CartItem>> GetByUserIdAsync(string userId)
            => await _context.CartItems
                .Include(ci => ci.Product)
                .Where(ci => ci.UserId == userId)
                .OrderByDescending(ci => ci.AddedAt)
                .ToListAsync();

        public async Task<CartItem?> GetByUserAndProductAsync(string userId, int productId)
            => await _context.CartItems
                .FirstOrDefaultAsync(ci => ci.UserId == userId && ci.ProductId == productId);

        public async Task<int> GetCartCountAsync(string userId)
            => await _context.CartItems
                .Where(ci => ci.UserId == userId)
                .SumAsync(ci => ci.Quantity);

        public async Task AddAsync(CartItem cartItem)
            => await _context.CartItems.AddAsync(cartItem);

        public void Update(CartItem cartItem)
            => _context.CartItems.Update(cartItem);

        public void Delete(CartItem cartItem)
            => _context.CartItems.Remove(cartItem);

        public async Task ClearCartAsync(string userId)
        {
            var items = await _context.CartItems.Where(ci => ci.UserId == userId).ToListAsync();
            _context.CartItems.RemoveRange(items);
        }

        public async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();
    }
}
