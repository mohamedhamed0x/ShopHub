using E_Commerce_mvc.Models.Entities;
using E_Commerce_mvc.ViewModels.Cart;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce_mvc.ViewModels.Checkout
{
    public class CheckoutVM
    {
        public List<CartItemVM> CartItems { get; set; } = new();
        public List<Address> SavedAddresses { get; set; } = new();
        public int? SelectedAddressId { get; set; }
        public decimal TotalAmount => CartItems.Sum(i => i.Subtotal);

        // New address fields
        [MaxLength(100)]
        public string? NewFullName { get; set; }

        [MaxLength(250)]
        public string? NewStreet { get; set; }

        [MaxLength(100)]
        public string? NewCity { get; set; }

        [MaxLength(100)]
        public string? NewState { get; set; }

        [MaxLength(20)]
        public string? NewZipCode { get; set; }

        [MaxLength(100)]
        public string? NewCountry { get; set; }

        [MaxLength(20)]
        public string? NewPhone { get; set; }
    }
}
