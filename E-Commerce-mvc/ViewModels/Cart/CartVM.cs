namespace E_Commerce_mvc.ViewModels.Cart
{
    public class CartVM
    {
        public List<CartItemVM> Items { get; set; } = new();
        public decimal TotalAmount => Items.Sum(i => i.Subtotal);
        public int ItemCount => Items.Sum(i => i.Quantity);
    }

    public class CartItemVM
    {
        public int CartItemId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public int StockQuantity { get; set; }
        public decimal Subtotal => UnitPrice * Quantity;
    }
}
