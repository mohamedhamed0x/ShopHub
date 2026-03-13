using E_Commerce_mvc.Services.Interfaces;
using E_Commerce_mvc.ViewModels.Catalog;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_mvc.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public CatalogController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index(int? categoryId, string? searchTerm, string? sortBy, int page = 1)
        {
            var (products, totalCount) = await _productService.GetFilteredProductsAsync(
                categoryId, searchTerm, sortBy, page);

            var categories = await _categoryService.GetActiveCategoriesAsync();
            int pageSize = 9;

            var viewModel = new ProductListVM
            {
                Products = products.Select(p => new ProductCardVM
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price = p.Price,
                    DiscountPrice = p.DiscountPrice,
                    ImageUrl = p.ImageUrl,
                    CategoryName = p.Category?.Name ?? "",
                    IsInStock = p.StockQuantity > 0
                }),
                Categories = categories,
                SelectedCategoryId = categoryId,
                SearchTerm = searchTerm,
                SortBy = sortBy,
                CurrentPage = page,
                TotalCount = totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null)
                return NotFound();

            var viewModel = new ProductDetailsVM
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                DiscountPrice = product.DiscountPrice,
                ImageUrl = product.ImageUrl,
                StockQuantity = product.StockQuantity,
                CategoryName = product.Category?.Name ?? "",
                CategoryId = product.CategoryId
            };

            return View(viewModel);
        }
    }
}
