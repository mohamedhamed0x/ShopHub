using E_Commerce_mvc.Models.Entities;

namespace E_Commerce_mvc.ViewModels.Catalog
{
    public class ProductListVM
    {
        public IEnumerable<ProductCardVM> Products { get; set; } = Enumerable.Empty<ProductCardVM>();
        public IEnumerable<Category> Categories { get; set; } = Enumerable.Empty<Category>();
        public int? SelectedCategoryId { get; set; }
        public string? SearchTerm { get; set; }
        public string? SortBy { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }
    }

    public class ProductCardVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public string? ImageUrl { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public bool IsInStock { get; set; }
        public decimal EffectivePrice => DiscountPrice ?? Price;
        public bool HasDiscount => DiscountPrice.HasValue && DiscountPrice < Price;
        public int DiscountPercentage => HasDiscount
            ? (int)Math.Round((1 - (DiscountPrice!.Value / Price)) * 100)
            : 0;
    }

    public class ProductDetailsVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountPrice { get; set; }
        public string? ImageUrl { get; set; }
        public int StockQuantity { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public bool IsInStock => StockQuantity > 0;
        public decimal EffectivePrice => DiscountPrice ?? Price;
        public bool HasDiscount => DiscountPrice.HasValue && DiscountPrice < Price;
        public int DiscountPercentage => HasDiscount
            ? (int)Math.Round((1 - (DiscountPrice!.Value / Price)) * 100)
            : 0;
    }
}
