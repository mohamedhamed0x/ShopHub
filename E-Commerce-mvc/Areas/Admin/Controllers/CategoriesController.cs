using E_Commerce_mvc.Models.Entities;
using E_Commerce_mvc.Services.Interfaces;
using E_Commerce_mvc.ViewModels.Admin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce_mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllCategoriesAsync();
            return View(categories);
        }

        public IActionResult Create()
        {
            return View(new CategoryFormVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryFormVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var category = new Category
            {
                Name = model.Name,
                Description = model.Description,
                IsActive = model.IsActive
            };

            await _categoryService.CreateCategoryAsync(category);
            TempData["Success"] = "Category created successfully!";
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryService.GetCategoryByIdAsync(id);
            if (category == null) return NotFound();

            var viewModel = new CategoryFormVM
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                IsActive = category.IsActive
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryFormVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var category = await _categoryService.GetCategoryByIdAsync(model.Id);
            if (category == null) return NotFound();

            category.Name = model.Name;
            category.Description = model.Description;
            category.IsActive = model.IsActive;

            await _categoryService.UpdateCategoryAsync(category);
            TempData["Success"] = "Category updated successfully!";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            await _categoryService.ToggleCategoryStatusAsync(id);
            TempData["Success"] = "Category status updated!";
            return RedirectToAction("Index");
        }
    }
}
