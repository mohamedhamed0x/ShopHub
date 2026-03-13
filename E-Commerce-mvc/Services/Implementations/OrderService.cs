using E_Commerce_mvc.Data;
using E_Commerce_mvc.Models.Entities;
using E_Commerce_mvc.Models.Enums;
using E_Commerce_mvc.Repositories.Interfaces;
using E_Commerce_mvc.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_mvc.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;
        private readonly AppDbContext _context;

        public OrderService(
            IOrderRepository orderRepository,
            ICartRepository cartRepository,
            IProductRepository productRepository,
            AppDbContext context)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
            => await _orderRepository.GetAllAsync();

        public async Task<IEnumerable<Order>> GetUserOrdersAsync(string userId)
            => await _orderRepository.GetByUserIdAsync(userId);

        public async Task<Order?> GetOrderDetailsAsync(int orderId)
            => await _orderRepository.GetByIdWithDetailsAsync(orderId);

        public async Task<Order> PlaceOrderAsync(string userId, int shippingAddressId)
        {
            // Use a transaction to ensure atomicity
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Get cart items
                var cartItems = (await _cartRepository.GetByUserIdAsync(userId)).ToList();
                if (!cartItems.Any())
                    throw new InvalidOperationException("Cart is empty.");

                // Validate stock for all items
                var stockErrors = new List<string>();
                foreach (var item in cartItems)
                {
                    var product = await _productRepository.GetByIdAsync(item.ProductId);
                    if (product == null || !product.IsActive)
                    {
                        stockErrors.Add($"Product '{item.Product.Name}' is no longer available.");
                    }
                    else if (product.StockQuantity < item.Quantity)
                    {
                        stockErrors.Add($"Only {product.StockQuantity} units of '{product.Name}' are available.");
                    }
                }

                if (stockErrors.Any())
                    throw new InvalidOperationException(string.Join(" ", stockErrors));

                // Calculate total
                decimal totalAmount = cartItems.Sum(ci =>
                    (ci.Product.DiscountPrice ?? ci.Product.Price) * ci.Quantity);

                // Create order
                var order = new Order
                {
                    UserId = userId,
                    ShippingAddressId = shippingAddressId,
                    OrderDate = DateTime.UtcNow,
                    TotalAmount = totalAmount,
                    Status = OrderStatus.Pending,
                    OrderItems = cartItems.Select(ci => new OrderItem
                    {
                        ProductId = ci.ProductId,
                        Quantity = ci.Quantity,
                        UnitPrice = ci.Product.DiscountPrice ?? ci.Product.Price
                    }).ToList()
                };

                await _orderRepository.AddAsync(order);

                // Decrement stock
                foreach (var item in cartItems)
                {
                    var product = await _productRepository.GetByIdAsync(item.ProductId);
                    if (product != null)
                    {
                        product.StockQuantity -= item.Quantity;
                        _productRepository.Update(product);
                    }
                }

                // Clear cart
                await _cartRepository.ClearCartAsync(userId);

                // Save and commit
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return order;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task UpdateOrderStatusAsync(int orderId, OrderStatus status)
        {
            var order = await _orderRepository.GetByIdAsync(orderId);
            if (order != null)
            {
                order.Status = status;
                _orderRepository.Update(order);
                await _orderRepository.SaveChangesAsync();
            }
        }

        public async Task CancelOrderAsync(int orderId, string userId)
        {
            var order = await _orderRepository.GetByIdWithDetailsAsync(orderId);
            if (order == null)
                throw new InvalidOperationException("Order not found.");
            if (order.UserId != userId)
                throw new UnauthorizedAccessException("You can only cancel your own orders.");
            if (order.Status != OrderStatus.Pending)
                throw new InvalidOperationException("Only pending orders can be cancelled.");

            // Restore stock
            foreach (var item in order.OrderItems)
            {
                var product = await _productRepository.GetByIdAsync(item.ProductId);
                if (product != null)
                {
                    product.StockQuantity += item.Quantity;
                    _productRepository.Update(product);
                }
            }

            order.Status = OrderStatus.Cancelled;
            _orderRepository.Update(order);
            await _orderRepository.SaveChangesAsync();
        }

        public async Task<int> GetOrderCountAsync()
            => await _orderRepository.GetCountAsync();

        public async Task<decimal> GetTotalRevenueAsync()
            => await _orderRepository.GetTotalRevenueAsync();

        public async Task<int> GetPendingOrderCountAsync()
            => await _orderRepository.GetCountByStatusAsync(OrderStatus.Pending);
    }
}
