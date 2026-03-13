using E_Commerce_mvc.Models.Entities;
using E_Commerce_mvc.Repositories.Interfaces;
using E_Commerce_mvc.Services.Interfaces;
using E_Commerce_mvc.ViewModels.Cart;

namespace E_Commerce_mvc.Services.Implementations
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;

        public CartService(ICartRepository cartRepository, IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        public async Task<CartVM> GetCartAsync(string userId)
        {
            var cartItems = await _cartRepository.GetByUserIdAsync(userId);
            return new CartVM
            {
                Items = cartItems.Select(ci => new CartItemVM
                {
                    CartItemId = ci.Id,
                    ProductId = ci.ProductId,
                    ProductName = ci.Product.Name,
                    ImageUrl = ci.Product.ImageUrl,
                    UnitPrice = ci.Product.DiscountPrice ?? ci.Product.Price,
                    Quantity = ci.Quantity,
                    StockQuantity = ci.Product.StockQuantity
                }).ToList()
            };
        }

        public async Task AddToCartAsync(string userId, int productId, int quantity = 1)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null || !product.IsActive || product.StockQuantity < quantity)
                throw new InvalidOperationException("Product is not available.");

            var existingItem = await _cartRepository.GetByUserAndProductAsync(userId, productId);
            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
                if (existingItem.Quantity > product.StockQuantity)
                    existingItem.Quantity = product.StockQuantity;
                _cartRepository.Update(existingItem);
            }
            else
            {
                var cartItem = new CartItem
                {
                    UserId = userId,
                    ProductId = productId,
                    Quantity = quantity,
                    AddedAt = DateTime.UtcNow
                };
                await _cartRepository.AddAsync(cartItem);
            }
            await _cartRepository.SaveChangesAsync();
        }

        public async Task UpdateQuantityAsync(string userId, int cartItemId, int quantity)
        {
            var cartItems = await _cartRepository.GetByUserIdAsync(userId);
            var item = cartItems.FirstOrDefault(ci => ci.Id == cartItemId);

            if (item != null)
            {
                if (quantity <= 0)
                {
                    _cartRepository.Delete(item);
                }
                else
                {
                    item.Quantity = Math.Min(quantity, item.Product.StockQuantity);
                    _cartRepository.Update(item);
                }
                await _cartRepository.SaveChangesAsync();
            }
        }

        public async Task RemoveFromCartAsync(string userId, int cartItemId)
        {
            var cartItems = await _cartRepository.GetByUserIdAsync(userId);
            var item = cartItems.FirstOrDefault(ci => ci.Id == cartItemId);

            if (item != null)
            {
                _cartRepository.Delete(item);
                await _cartRepository.SaveChangesAsync();
            }
        }

        public async Task<int> GetCartCountAsync(string userId)
            => await _cartRepository.GetCartCountAsync(userId);

        public async Task ClearCartAsync(string userId)
        {
            await _cartRepository.ClearCartAsync(userId);
            await _cartRepository.SaveChangesAsync();
        }
    }
}
