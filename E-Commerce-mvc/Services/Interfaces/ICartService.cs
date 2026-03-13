using E_Commerce_mvc.Models.Entities;
using E_Commerce_mvc.ViewModels.Cart;

namespace E_Commerce_mvc.Services.Interfaces
{
    public interface ICartService
    {
        Task<CartVM> GetCartAsync(string userId);
        Task AddToCartAsync(string userId, int productId, int quantity = 1);
        Task UpdateQuantityAsync(string userId, int cartItemId, int quantity);
        Task RemoveFromCartAsync(string userId, int cartItemId);
        Task<int> GetCartCountAsync(string userId);
        Task ClearCartAsync(string userId);
    }
}
