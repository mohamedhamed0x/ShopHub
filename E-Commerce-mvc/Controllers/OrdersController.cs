using E_Commerce_mvc.Models.Enums;
using E_Commerce_mvc.Services.Interfaces;
using E_Commerce_mvc.ViewModels.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce_mvc.Controllers
{
    [Authorize]
    public class OrdersController : Controller
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        public async Task<IActionResult> Index()
        {
            var orders = await _orderService.GetUserOrdersAsync(GetUserId());
            var viewModel = new OrderListVM
            {
                Orders = orders.Select(o => new OrderSummaryVM
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

        public async Task<IActionResult> Details(int id)
        {
            var order = await _orderService.GetOrderDetailsAsync(id);
            if (order == null || order.UserId != GetUserId())
                return NotFound();

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
                    ProductName = oi.Product?.Name ?? "Product Unavailable",
                    ImageUrl = oi.Product?.ImageUrl,
                    UnitPrice = oi.UnitPrice,
                    Quantity = oi.Quantity
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            try
            {
                await _orderService.CancelOrderAsync(id, GetUserId());
                TempData["Success"] = "Order cancelled successfully.";
            }
            catch (Exception ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("Details", new { id });
        }
    }
}
