using E_Commerce_mvc.Models.Entities;
using E_Commerce_mvc.Repositories.Interfaces;
using E_Commerce_mvc.Services.Interfaces;

namespace E_Commerce_mvc.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
            => await _categoryRepository.GetAllAsync();

        public async Task<IEnumerable<Category>> GetActiveCategoriesAsync()
            => await _categoryRepository.GetActiveAsync();

        public async Task<Category?> GetCategoryByIdAsync(int id)
            => await _categoryRepository.GetByIdAsync(id);

        public async Task<Category?> GetCategoryWithProductsAsync(int id)
            => await _categoryRepository.GetByIdWithProductsAsync(id);

        public async Task CreateCategoryAsync(Category category)
        {
            await _categoryRepository.AddAsync(category);
            await _categoryRepository.SaveChangesAsync();
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            _categoryRepository.Update(category);
            await _categoryRepository.SaveChangesAsync();
        }

        public async Task ToggleCategoryStatusAsync(int id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category != null)
            {
                category.IsActive = !category.IsActive;
                _categoryRepository.Update(category);
                await _categoryRepository.SaveChangesAsync();
            }
        }
    }
}
