using E_Commerce_mvc.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce_mvc.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        private string GetUserId() => User.FindFirstValue(ClaimTypes.NameIdentifier)!;

        public async Task<IActionResult> Index()
        {
            var cart = await _cartService.GetCartAsync(GetUserId());
            return View(cart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            try
            {
                await _cartService.AddToCartAsync(GetUserId(), productId, quantity);
                TempData["Success"] = "Item added to cart!";
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateQuantity(int cartItemId, int quantity)
        {
            await _cartService.UpdateQuantityAsync(GetUserId(), cartItemId, quantity);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveItem(int cartItemId)
        {
            await _cartService.RemoveFromCartAsync(GetUserId(), cartItemId);
            TempData["Success"] = "Item removed from cart.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> GetCartCount()
        {
            var count = await _cartService.GetCartCountAsync(GetUserId());
            return Json(new { count });
        }
    }
}
