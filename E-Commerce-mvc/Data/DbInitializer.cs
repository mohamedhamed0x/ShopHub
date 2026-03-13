using E_Commerce_mvc.Models.Entities;
using Microsoft.AspNetCore.Identity;

namespace E_Commerce_mvc.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // Seed Roles
            string[] roles = { "Admin", "Customer" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Seed Admin User
            var adminEmail = "admin@ecommerce.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Admin",
                    LastName = "User",
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, "Admin@123");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            // Seed Categories
            var context = serviceProvider.GetRequiredService<AppDbContext>();
            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category { Name = "Electronics", Description = "Electronic devices and gadgets", IsActive = true },
                    new Category { Name = "Clothing", Description = "Men's and women's clothing", IsActive = true },
                    new Category { Name = "Books", Description = "Physical and digital books", IsActive = true },
                    new Category { Name = "Home & Garden", Description = "Home decor and garden supplies", IsActive = true },
                    new Category { Name = "Sports", Description = "Sports equipment and accessories", IsActive = true }
                };
                context.Categories.AddRange(categories);
                await context.SaveChangesAsync();
            }

            // Seed Products
            if (!context.Products.Any())
            {
                var electronics = context.Categories.First(c => c.Name == "Electronics");
                var clothing = context.Categories.First(c => c.Name == "Clothing");
                var books = context.Categories.First(c => c.Name == "Books");
                var home = context.Categories.First(c => c.Name == "Home & Garden");
                var sports = context.Categories.First(c => c.Name == "Sports");

                var products = new List<Product>
                {
                    new Product { Name = "Wireless Bluetooth Headphones", Description = "High-quality wireless headphones with noise cancellation and 30-hour battery life.", Price = 79.99m, DiscountPrice = 59.99m, StockQuantity = 50, CategoryId = electronics.Id, ImageUrl = "/images/products/headphones.jpg" },
                    new Product { Name = "Smartphone Stand", Description = "Adjustable aluminum smartphone stand for desk use.", Price = 24.99m, StockQuantity = 100, CategoryId = electronics.Id, ImageUrl = "/images/products/phone-stand.jpg" },
                    new Product { Name = "USB-C Hub Adapter", Description = "7-in-1 USB-C hub with HDMI, USB 3.0, and SD card reader.", Price = 45.99m, DiscountPrice = 39.99m, StockQuantity = 75, CategoryId = electronics.Id, ImageUrl = "/images/products/usb-hub.jpg" },
                    new Product { Name = "Classic Denim Jacket", Description = "Timeless denim jacket perfect for casual wear.", Price = 89.99m, StockQuantity = 30, CategoryId = clothing.Id, ImageUrl = "/images/products/denim-jacket.jpg" },
                    new Product { Name = "Cotton T-Shirt Pack", Description = "Pack of 3 premium cotton t-shirts in neutral colors.", Price = 34.99m, DiscountPrice = 29.99m, StockQuantity = 200, CategoryId = clothing.Id, ImageUrl = "/images/products/tshirt-pack.jpg" },
                    new Product { Name = "Running Sneakers", Description = "Lightweight running shoes with cushioned soles.", Price = 119.99m, DiscountPrice = 99.99m, StockQuantity = 45, CategoryId = clothing.Id, ImageUrl = "/images/products/sneakers.jpg" },
                    new Product { Name = "Clean Code", Description = "A Handbook of Agile Software Craftsmanship by Robert C. Martin.", Price = 39.99m, StockQuantity = 60, CategoryId = books.Id, ImageUrl = "/images/products/clean-code.jpg" },
                    new Product { Name = "Design Patterns", Description = "Elements of Reusable Object-Oriented Software by Gang of Four.", Price = 44.99m, StockQuantity = 40, CategoryId = books.Id, ImageUrl = "/images/products/design-patterns.jpg" },
                    new Product { Name = "LED Desk Lamp", Description = "Modern LED desk lamp with adjustable brightness and color temperature.", Price = 54.99m, DiscountPrice = 44.99m, StockQuantity = 35, CategoryId = home.Id, ImageUrl = "/images/products/desk-lamp.jpg" },
                    new Product { Name = "Ceramic Plant Pots Set", Description = "Set of 3 minimalist ceramic plant pots in varying sizes.", Price = 32.99m, StockQuantity = 80, CategoryId = home.Id, ImageUrl = "/images/products/plant-pots.jpg" },
                    new Product { Name = "Yoga Mat", Description = "Non-slip yoga mat with carrying strap, 6mm thick.", Price = 29.99m, StockQuantity = 90, CategoryId = sports.Id, ImageUrl = "/images/products/yoga-mat.jpg" },
                    new Product { Name = "Dumbbell Set", Description = "Adjustable dumbbell set from 5 to 25 lbs.", Price = 149.99m, DiscountPrice = 129.99m, StockQuantity = 20, CategoryId = sports.Id, ImageUrl = "/images/products/dumbbells.jpg" }
                };
                context.Products.AddRange(products);
                await context.SaveChangesAsync();
            }
        }
    }
}
