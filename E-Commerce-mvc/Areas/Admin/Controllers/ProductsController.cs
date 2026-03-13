using E_Commerce_mvc.Models.Entities;
using E_Commerce_mvc.Services.Interfaces;
using E_Commerce_mvc.ViewModels.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductsController(
            IProductService productService,
            ICategoryService categoryService,
            IWebHostEnvironment webHostEnvironment)
        {
            _productService = productService;
            _categoryService = categoryService;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllProductsAsync();
            return View(products);
        }

        public async Task<IActionResult> Create()
        {
            var viewModel = new ProductFormVM
            {
                Categories = await _categoryService.GetAllCategoriesAsync()
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductFormVM model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = await _categoryService.GetAllCategoriesAsync();
                return View(model);
            }

            var product = new Product
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                DiscountPrice = model.DiscountPrice,
                StockQuantity = model.StockQuantity,
                CategoryId = model.CategoryId,
                IsActive = model.IsActive
            };

            // Handle image upload
            if (model.ImageFile != null)
            {
                product.ImageUrl = await SaveImageAsync(model.ImageFile);
            }

            await _productService.CreateProductAsync(product);
            TempData["Success"] = "Product created successfully!";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);
            if (product == null) return NotFound();

            var viewModel = new ProductFormVM
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                DiscountPrice = product.DiscountPrice,
                StockQuantity = product.StockQuantity,
                CategoryId = product.CategoryId,
                ImageUrl = product.ImageUrl,
                IsActive = product.IsActive,
                Categories = await _categoryService.GetAllCategoriesAsync()
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductFormVM model)
        {
            if (!ModelState.IsValid)
            {
                model.Categories = await _categoryService.GetAllCategoriesAsync();
                return View(model);
            }

            var product = await _productService.GetProductByIdAsync(model.Id);
            if (product == null) return NotFound();

            product.Name = model.Name;
            product.Description = model.Description;
            product.Price = model.Price;
            product.DiscountPrice = model.DiscountPrice;
            product.StockQuantity = model.StockQuantity;
            product.CategoryId = model.CategoryId;
            product.IsActive = model.IsActive;

            if (model.ImageFile != null)
            {
                product.ImageUrl = await SaveImageAsync(model.ImageFile);
            }

            await _productService.UpdateProductAsync(product);
            TempData["Success"] = "Product updated successfully!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _productService.ToggleProductStatusAsync(id);
            TempData["Success"] = "Product status updated!";
            return RedirectToAction("Index");
        }

        private async Task<string> SaveImageAsync(IFormFile file)
        {
            var uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "images", "products");
            Directory.CreateDirectory(uploadsDir);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsDir, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/images/products/{fileName}";
        }
    }
}
