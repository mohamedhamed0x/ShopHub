using E_Commerce_mvc.Models.Enums;
using E_Commerce_mvc.Services.Interfaces;
using E_Commerce_mvc.ViewModels.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<IActionResult> Index(OrderStatus? status)
        {
            var orders = await _orderService.GetAllOrdersAsync();
            if (status.HasValue)
                orders = orders.Where(o => o.Status == status.Value);

            ViewBag.CurrentStatus = status;
            return View(orders);
        }

        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderService.GetOrderDetailsAsync(id);
            if (order == null) return NotFound();

            var viewModel = new OrderDetailsVM
            {
                OrderId = order.Id,
                OrderDate = order.OrderDate,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                Notes = order.Notes,
                CustomerName = order.User?.FirstName + " " + order.User?.LastName,
                CustomerEmail = order.User?.Email ?? "",
                ShippingAddress = new OrderAddressVM
                {
                    FullName = order.ShippingAddress.FullName,
                    Street = order.ShippingAddress.Street,
                    City = order.ShippingAddress.City,
                    State = order.ShippingAddress.State,
                    ZipCode = order.ShippingAddress.ZipCode,
                    Country = order.ShippingAddress.Country,
                    Phone = order.ShippingAddress.Phone
                },
                Items = order.OrderItems.Select(oi => new OrderItemVM
                {
                    ProductName = oi.Product?.Name ?? "N/A",
                    ImageUrl = oi.Product?.ImageUrl,
                    UnitPrice = oi.UnitPrice,
                    Quantity = oi.Quantity
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int orderId, OrderStatus newStatus)
        {
            await _orderService.UpdateOrderStatusAsync(orderId, newStatus);
            TempData["Success"] = "Order status updated!";
            return RedirectToAction("Details", new { id = orderId });
        }
    }
}
