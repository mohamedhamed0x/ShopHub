using E_Commerce_mvc.Models.Enums;

namespace E_Commerce_mvc.ViewModels.Orders
{
    public class OrderListVM
    {
        public List<OrderSummaryVM> Orders { get; set; } = new();
    }

    public class OrderSummaryVM
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public decimal TotalAmount { get; set; }
        public int ItemCount { get; set; }
    }

    public class OrderDetailsVM
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public decimal TotalAmount { get; set; }
        public string? Notes { get; set; }
        public List<OrderItemVM> Items { get; set; } = new();
        public OrderAddressVM ShippingAddress { get; set; } = new();
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerEmail { get; set; } = string.Empty;
    }

    public class OrderItemVM
    {
        public string ProductName { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Subtotal => UnitPrice * Quantity;
    }

    public class OrderAddressVM
    {
        public string FullName { get; set; } = string.Empty;
        public string Street { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string? State { get; set; }
        public string ZipCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string? Phone { get; set; }
    }
}
