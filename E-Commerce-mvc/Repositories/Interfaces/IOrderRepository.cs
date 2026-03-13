using E_Commerce_mvc.Models.Entities;
using E_Commerce_mvc.Models.Enums;

namespace E_Commerce_mvc.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<IEnumerable<Order>> GetByUserIdAsync(string userId);
        Task<IEnumerable<Order>> GetByStatusAsync(OrderStatus status);
        Task<Order?> GetByIdAsync(int id);
        Task<Order?> GetByIdWithDetailsAsync(int id);
        Task AddAsync(Order order);
        void Update(Order order);
        Task<int> GetCountAsync();
        Task<decimal> GetTotalRevenueAsync();
        Task<int> GetCountByStatusAsync(OrderStatus status);
        Task SaveChangesAsync();
    }
}
