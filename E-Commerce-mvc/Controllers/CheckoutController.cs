using E_Commerce_mvc.Models.Entities;
using E_Commerce_mvc.Repositories.Interfaces;
using E_Commerce_mvc.Services.Interfaces;
using E_Commerce_mvc.ViewModels.Cart;
using E_Commerce_mvc.ViewModels.Checkout;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce_mvc.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;
        private readonly IAddressRepository _addressRepository;

        public CheckoutController(
            ICartService cartService,
            IOrderService orderService,
            IAddressRepository addressRepository)
        {
            _cartService = cartService;
            _orderService = orderService;
            _addressRepository = addressRepository;
        }

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        public async Task<IActionResult> Index()
        {
            var cart = await _cartService.GetCartAsync(GetUserId());
            if (!cart.Items.Any())
            {
                TempData["Error"] = "Your cart is empty.";
                return RedirectToAction("Index", "Cart");
            }

            var addresses = await _addressRepository.GetByUserIdAsync(GetUserId());
            var viewModel = new CheckoutVM
            {
                CartItems = cart.Items,
                SavedAddresses = addresses.ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PlaceOrder(CheckoutVM model)
        {
            var userId = GetUserId();
            int addressId;

            // Handle address selection or creation
            if (model.SelectedAddressId.HasValue && model.SelectedAddressId > 0)
            {
                addressId = model.SelectedAddressId.Value;
            }
            else if (!string.IsNullOrWhiteSpace(model.NewStreet))
            {
                // Create new address
                var address = new Address
                {
                    UserId = userId,
                    FullName = model.NewFullName ?? "",
                    Street = model.NewStreet,
                    City = model.NewCity ?? "",
                    State = model.NewState,
                    ZipCode = model.NewZipCode ?? "",
                    Country = model.NewCountry ?? "",
                    Phone = model.NewPhone
                };
                await _addressRepository.AddAsync(address);
                await _addressRepository.SaveChangesAsync();
                addressId = address.Id;
            }
            else
            {
                TempData["Error"] = "Please select or enter a shipping address.";
                return RedirectToAction("Index");
            }

            try
            {
                var order = await _orderService.PlaceOrderAsync(userId, addressId);
                return RedirectToAction("Confirmation", new { orderId = order.Id });
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Confirmation(int orderId)
        {
            var order = await _orderService.GetOrderDetailsAsync(orderId);
            if (order == null || order.UserId != GetUserId())
                return NotFound();

            return View(order);
        }
    }
}
