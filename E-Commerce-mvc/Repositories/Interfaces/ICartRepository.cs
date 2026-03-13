using E_Commerce_mvc.Models.Entities;

namespace E_Commerce_mvc.Repositories.Interfaces
{
    public interface ICartRepository
    {
        Task<IEnumerable<CartItem>> GetByUserIdAsync(string userId);
        Task<CartItem?> GetByUserAndProductAsync(string userId, int productId);
        Task<int> GetCartCountAsync(string userId);
        Task AddAsync(CartItem cartItem);
        void Update(CartItem cartItem);
        void Delete(CartItem cartItem);
        Task ClearCartAsync(string userId);
        Task SaveChangesAsync();
    }
}
