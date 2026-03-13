using E_Commerce_mvc.Models.Enums;
using E_Commerce_mvc.Services.Interfaces;
using E_Commerce_mvc.ViewModels.Admin;
using E_Commerce_mvc.ViewModels.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly IProductService _productService;

        public DashboardController(IOrderService orderService, IProductService productService)
        {
            _orderService = orderService;
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var recentOrders = await _orderService.GetAllOrdersAsync();

            var viewModel = new DashboardVM
            {
                TotalOrders = await _orderService.GetOrderCountAsync(),
                TotalRevenue = await _orderService.GetTotalRevenueAsync(),
                TotalProducts = await _productService.GetProductCountAsync(),
                PendingOrders = await _orderService.GetPendingOrderCountAsync(),
                RecentOrders = recentOrders.Take(5).Select(o => new OrderSummaryVM
                {
                    OrderId = o.Id,
                    OrderDate = o.OrderDate,
                    Status = o.Status,
                    TotalAmount = o.TotalAmount,
                    ItemCount = o.OrderItems.Count
                }).ToList()
            };

            return View(viewModel);
        }
    }
}
