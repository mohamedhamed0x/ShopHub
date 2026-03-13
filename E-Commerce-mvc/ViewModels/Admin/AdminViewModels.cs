using E_Commerce_mvc.Models.Entities;
using E_Commerce_mvc.Models.Enums;
using E_Commerce_mvc.ViewModels.Orders;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce_mvc.ViewModels.Admin
{
    public class ProductFormVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Product name is required")]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 999999.99, ErrorMessage = "Price must be between 0.01 and 999,999.99")]
        public decimal Price { get; set; }

        [Range(0.01, 999999.99, ErrorMessage = "Discount price must be between 0.01 and 999,999.99")]
        [Display(Name = "Discount Price")]
        public decimal? DiscountPrice { get; set; }

        [Required(ErrorMessage = "Stock quantity is required")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock quantity must be 0 or more")]
        [Display(Name = "Stock Quantity")]
        public int StockQuantity { get; set; }

        [Required(ErrorMessage = "Category is required")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public string? ImageUrl { get; set; }

        [Display(Name = "Product Image")]
        public IFormFile? ImageFile { get; set; }

        public bool IsActive { get; set; } = true;

        // For dropdown
        public IEnumerable<Category> Categories { get; set; } = Enumerable.Empty<Category>();
    }

    public class CategoryFormVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Category name is required")]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500)]
        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        [Display(Name = "Category Image")]
        public IFormFile? ImageFile { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;
    }

    public class DashboardVM
    {
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public int TotalProducts { get; set; }
        public int PendingOrders { get; set; }
        public List<OrderSummaryVM> RecentOrders { get; set; } = new();
    }

    public class AdminOrderVM
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public decimal TotalAmount { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
        public int ItemCount { get; set; }
        public OrderStatus NewStatus { get; set; }
    }
}
