using E_Commerce_mvc.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public HomeController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var (products, _) = await _productService.GetFilteredProductsAsync(null, null, "newest", 1, 8);
            var categories = await _categoryService.GetActiveCategoriesAsync();
            ViewBag.Categories = categories;
            ViewBag.FeaturedProducts = products;
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Contact()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}
