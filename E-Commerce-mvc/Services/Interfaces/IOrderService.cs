using E_Commerce_mvc.Models.Entities;
using E_Commerce_mvc.Models.Enums;
using E_Commerce_mvc.ViewModels.Orders;

namespace E_Commerce_mvc.Services.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<IEnumerable<Order>> GetUserOrdersAsync(string userId);
        Task<Order?> GetOrderDetailsAsync(int orderId);
        Task<Order> PlaceOrderAsync(string userId, int shippingAddressId);
        Task UpdateOrderStatusAsync(int orderId, OrderStatus status);
        Task CancelOrderAsync(int orderId, string userId);
        Task<int> GetOrderCountAsync();
        Task<decimal> GetTotalRevenueAsync();
        Task<int> GetPendingOrderCountAsync();
    }
}
