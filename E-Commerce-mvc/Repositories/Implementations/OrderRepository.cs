using E_Commerce_mvc.Data;
using E_Commerce_mvc.Models.Entities;
using E_Commerce_mvc.Models.Enums;
using E_Commerce_mvc.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce_mvc.Repositories.Implementations
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
            => await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

        public async Task<IEnumerable<Order>> GetByUserIdAsync(string userId)
            => await _context.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

        public async Task<IEnumerable<Order>> GetByStatusAsync(OrderStatus status)
            => await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .Where(o => o.Status == status)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

        public async Task<Order?> GetByIdAsync(int id)
            => await _context.Orders
                .Include(o => o.User)
                .FirstOrDefaultAsync(o => o.Id == id);

        public async Task<Order?> GetByIdWithDetailsAsync(int id)
            => await _context.Orders
                .Include(o => o.User)
                .Include(o => o.ShippingAddress)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

        public async Task AddAsync(Order order)
            => await _context.Orders.AddAsync(order);

        public void Update(Order order)
            => _context.Orders.Update(order);

        public async Task<int> GetCountAsync()
            => await _context.Orders.CountAsync();

        public async Task<decimal> GetTotalRevenueAsync()
            => await _context.Orders
                .Where(o => o.Status != OrderStatus.Cancelled)
                .SumAsync(o => o.TotalAmount);

        public async Task<int> GetCountByStatusAsync(OrderStatus status)
            => await _context.Orders.CountAsync(o => o.Status == status);

        public async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();
    }
}
