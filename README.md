# ShopHub - E-Commerce MVC Application

A modern, full-featured e-commerce platform built with **ASP.NET Core MVC** and **Entity Framework Core**, featuring user authentication, product catalog management, shopping cart functionality, and order processing.

![Language Composition](https://img.shields.io/badge/C%23-58.5%25-239120?logo=csharp) ![HTML](https://img.shields.io/badge/HTML-36.7%25-E34C26?logo=html5) ![CSS](https://img.shields.io/badge/CSS-4.7%25-1572B6?logo=css3) ![JavaScript](https://img.shields.io/badge/JavaScript-0.1%25-F7DF1E?logo=javascript)

## 📋 Table of Contents

- [Features](#-features)
- [Technology Stack](#-technology-stack)
- [Project Structure](#-project-structure)
- [Prerequisites](#-prerequisites)
- [Installation & Setup](#-installation--setup)
- [Architecture](#-architecture)
- [Database Schema](#-database-schema)
- [API Endpoints](#-api-endpoints)
- [Configuration](#-configuration)
- [Contributing](#-contributing)
- [License](#-license)

## ✨ Features

### User Management
- **User Authentication**: Secure registration and login using ASP.NET Core Identity
- **Account Management**: User profile management and password security
- **Role-Based Access Control**: Different user roles for customers and administrators

### Product Catalog
- **Category Management**: Organize products by categories
- **Product Management**: Browse, search, and filter products
- **Product Details**: Detailed product information with pricing and inventory management

### Shopping Experience
- **Shopping Cart**: Add, remove, and update product quantities
- **Cart Persistence**: Session-based cart management
- **Checkout Process**: Secure and streamlined checkout flow

### Order Management
- **Order Creation**: Convert cart items into orders
- **Order Tracking**: View order history and status
- **Address Management**: Save and manage shipping addresses
- **Order Status**: Track order fulfillment status

### Admin Features
- **Admin Dashboard**: Administrative area for store management
- **Product Management**: Add, edit, and delete products
- **Category Management**: Manage product categories
- **Order Management**: View and process customer orders

## 🛠 Technology Stack

### Backend
- **Framework**: ASP.NET Core 8.0
- **ORM**: Entity Framework Core 8.0.11
- **Authentication**: ASP.NET Core Identity
- **Database**: SQL Server
- **Language**: C#

### Frontend
- **View Engine**: Razor (CSHTML)
- **Styling**: CSS3
- **Markup**: HTML5
- **Client-side**: Minimal JavaScript

### Architecture
- **Pattern**: MVC (Model-View-Controller)
- **Design Patterns**: Repository Pattern, Service Layer, Dependency Injection
- **Code Organization**: Clean separation of concerns

## 📁 Project Structure

```
ShopHub/
├── E-Commerce-mvc/
│   ├── Controllers/              # MVC Controllers
│   │   ├── AccountController.cs
│   │   ├── CartController.cs
│   │   ├── CatalogController.cs
│   │   ├── CheckoutController.cs
│   │   ├── HomeController.cs
│   │   └── OrdersController.cs
│   ├── Models/                   # Data Models
│   │   ├── Entities/             # Database entities
│   │   │   ├── Address.cs
│   │   │   ├── ApplicationUser.cs
│   │   │   ├── CartItem.cs
│   │   │   ├── Category.cs
│   │   │   ├── Order.cs
│   │   │   ├── OrderItem.cs
│   │   │   └── Product.cs
│   │   ├── Enums/                # Enumeration types
│   │   └─�� ErrorViewModel.cs
│   ├── Views/                    # Razor Views (CSHTML)
│   │   ├── Account/
│   │   ├── Cart/
│   │   ├── Catalog/
│   │   ├── Checkout/
│   │   ├── Home/
│   │   ├── Orders/
│   │   └── Shared/               # Shared layouts and components
│   ├── Services/                 # Business Logic Layer
│   │   ├── Interfaces/           # Service contracts
│   │   │   ├── ICartService.cs
│   │   │   ├── ICategoryService.cs
│   │   │   ├── IOrderService.cs
│   │   │   └── IProductService.cs
│   │   └── Implementations/      # Service implementations
│   │       ├── CartService.cs
│   │       ├── CategoryService.cs
│   │       ├── OrderService.cs
│   │       └── ProductService.cs
│   ├── Repositories/             # Data Access Layer
│   │   ├── Interfaces/           # Repository contracts
│   │   └── Implementations/      # Repository implementations
│   ├── Data/                     # Database context and seed data
│   │   └── AppDbContext.cs
│   ├── Migrations/               # EF Core migrations
│   ├── Areas/                    # Admin area (if applicable)
│   ├── wwwroot/                  # Static files (CSS, JS, images)
│   ├── Properties/               # Project properties
│   ├── Program.cs                # Application startup configuration
│   ├── appsettings.json          # Configuration settings
│   ├── appsettings.Development.json
│   └── E-Commerce-mvc.csproj     # Project file
├── E-Commerce.sln                # Solution file
└── README.md
```

## 📋 Prerequisites

- .NET 8.0 SDK or later
- SQL Server (Local or Remote)
- Visual Studio 2022 or Visual Studio Code
- Git

## 🚀 Installation & Setup

### 1. Clone the Repository
```bash
git clone https://github.com/mohamedhamed0x/ShopHub.git
cd ShopHub
```

### 2. Configure Database Connection
Edit `E-Commerce-mvc/appsettings.json` and update the connection string:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=ShopHubDb;Trusted_Connection=true;"
  }
}
```

### 3. Apply Database Migrations
```bash
cd E-Commerce-mvc
dotnet ef database update
```

### 4. Build the Solution
```bash
dotnet build
```

### 5. Run the Application
```bash
dotnet run
```

The application will be available at `https://localhost:5001` (or the configured port).

## 🏗 Architecture

### Layered Architecture
The application follows a clean layered architecture:

1. **Presentation Layer (Views & Controllers)**
   - Razor Views for UI
   - Controllers handling HTTP requests

2. **Service Layer**
   - Business logic implementation
   - Service interfaces for abstraction
   - SOLID principles compliance

3. **Repository Layer**
   - Data access abstraction
   - Generic repository pattern
   - Entity Framework Core integration

4. **Data Layer**
   - Entity models
   - Database context (AppDbContext)
   - Database migrations

### Dependency Injection
The application uses ASP.NET Core's built-in dependency injection container, configured in `Program.cs`:

```csharp
// Repositories
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();

// Services
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ICartService, CartService>();
```

## 💾 Database Schema

### Core Entities

**ApplicationUser**
- Extends IdentityUser
- Stores customer information
- Links to orders and addresses

**Product**
- ProductId (PK)
- Name, Description, Price
- Stock quantity
- Category relationship
- Image URL

**Category**
- CategoryId (PK)
- Name, Description
- One-to-Many relationship with Products

**Order**
- OrderId (PK)
- UserId (FK)
- OrderDate, Status
- Total amount
- Shipping address

**OrderItem**
- OrderItemId (PK)
- OrderId (FK)
- ProductId (FK)
- Quantity, Unit price

**CartItem**
- CartItemId (PK)
- UserId (FK)
- ProductId (FK)
- Quantity
- Session-based management

**Address**
- AddressId (PK)
- UserId (FK)
- Street, City, PostalCode, Country
- Multiple addresses per user

## 🔐 Authentication & Security

### Password Requirements
- Minimum 6 characters
- Requires uppercase letter
- Requires lowercase letter
- Requires digit
- Requires non-alphanumeric character
- Unique email requirement

### Cookie Configuration
- HttpOnly cookies (XSS protection)
- SameSite=Strict (CSRF protection)
- 60-minute expiration
- Sliding expiration enabled
- Secure policy for HTTPS

## 📱 Main Features

### Account Management
- Register new accounts with validation
- Secure login
- Password reset functionality
- User profile management

### Product Browsing
- Browse all products
- Filter by category
- View product details
- Product ratings and reviews

### Shopping Cart
- Add items to cart
- Update quantities
- Remove items
- Cart persistence across sessions

### Checkout & Orders
- Address selection/entry
- Order review
- Order confirmation
- Order history and tracking

### Admin Dashboard
- Manage products
- Manage categories
- View orders
- User management

## ⚙️ Configuration

### appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "your_connection_string"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information"
    }
  }
}
```

### Program.cs Key Configuration
- Database context setup with SQL Server
- Identity configuration with custom password requirements
- Cookie authentication settings
- Dependency injection registration
- Database seeding with initial data

## 📖 Usage Examples

### Adding a Product to Cart
```
POST /Cart/AddToCart/{productId}
```

### Viewing Shopping Cart
```
GET /Cart/ViewCart
```

### Processing Checkout
```
POST /Checkout/ProcessCheckout
```

### Viewing Order History
```
GET /Orders/MyOrders
```

## 🤝 Contributing

Contributions are welcome! Please follow these steps:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📝 License

This project is open source and available under the MIT License.

## 👨‍💻 Author

**Mohamed Hamed**
- GitHub: [@mohamedhamed0x](https://github.com/mohamedhamed0x)

## 📞 Support

For issues, questions, or suggestions, please open an issue on the GitHub repository.

---

**Happy Shopping! 🛍️**
