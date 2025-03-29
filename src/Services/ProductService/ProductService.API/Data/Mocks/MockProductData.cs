using ProductService.API.Models;
using System;
using System.Collections.Generic;

namespace ProductService.API.Data.Mocks
{
    public static class MockProductData
    {
        public static List<Category> GetMockCategories()
        {
            return new List<Category>
            {
                new Category { Id = "6507e9a33c4c9b9d6e4c83b1", Name = "Electronics", Description = "Electronic devices and gadgets" },
                new Category { Id = "6507e9a33c4c9b9d6e4c83b2", Name = "Clothing", Description = "Men's and women's apparel" },
                new Category { Id = "6507e9a33c4c9b9d6e4c83b3", Name = "Home", Description = "Home decor and furniture" },
                new Category { Id = "6507e9a33c4c9b9d6e4c83b4", Name = "Books", Description = "Books and literature" }
            };
        }

        public static List<Product> GetMockProducts()
        {
            var categories = GetMockCategories();
            var electronicsId = categories[0].Id;
            var clothingId = categories[1].Id;
            var homeId = categories[2].Id;
            var booksId = categories[3].Id;

            return new List<Product>
            {
                new Product
                {
                    Id = "6507e9a33c4c9b9d6e4c83c1",
                    Name = "Smartphone X",
                    Description = "Latest smartphone with advanced features",
                    Price = 799.99m,
                    ImageUrl = "/images/smartphone-x.jpg",
                    CategoryId = electronicsId,
                    Category = categories[0],
                    IsFeatured = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-30),
                    StockQuantity = 50
                },
                new Product
                {
                    Id = "6507e9a33c4c9b9d6e4c83c2",
                    Name = "Laptop Pro",
                    Description = "High-performance laptop for professionals",
                    Price = 1299.99m,
                    ImageUrl = "/images/laptop-pro.jpg",
                    CategoryId = electronicsId,
                    Category = categories[0],
                    IsFeatured = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-60),
                    StockQuantity = 25
                },
                new Product
                {
                    Id = "6507e9a33c4c9b9d6e4c83c3",
                    Name = "Designer T-Shirt",
                    Description = "Comfortable cotton t-shirt with modern design",
                    Price = 29.99m,
                    ImageUrl = "/images/tshirt.jpg",
                    CategoryId = clothingId,
                    Category = categories[1],
                    IsFeatured = false,
                    CreatedAt = DateTime.UtcNow.AddDays(-15),
                    StockQuantity = 100
                },
                new Product
                {
                    Id = "6507e9a33c4c9b9d6e4c83c4",
                    Name = "Wireless Earbuds",
                    Description = "Premium sound quality with noise cancellation",
                    Price = 129.99m,
                    ImageUrl = "/images/earbuds.jpg",
                    CategoryId = electronicsId,
                    Category = categories[0],
                    IsFeatured = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-5),
                    StockQuantity = 75
                },
                new Product
                {
                    Id = "6507e9a33c4c9b9d6e4c83c5",
                    Name = "Smart Watch",
                    Description = "Fitness and health tracking smartwatch",
                    Price = 199.99m,
                    ImageUrl = "/images/smartwatch.jpg",
                    CategoryId = electronicsId,
                    Category = categories[0],
                    IsFeatured = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-2),
                    StockQuantity = 40
                },
                new Product
                {
                    Id = "6507e9a33c4c9b9d6e4c83c6",
                    Name = "Modern Coffee Table",
                    Description = "Stylish coffee table for your living room",
                    Price = 249.99m,
                    ImageUrl = "/images/coffee-table.jpg",
                    CategoryId = homeId,
                    Category = categories[2],
                    IsFeatured = false,
                    CreatedAt = DateTime.UtcNow.AddDays(-45),
                    StockQuantity = 15
                },
                new Product
                {
                    Id = "6507e9a33c4c9b9d6e4c83c7",
                    Name = "Bestselling Novel",
                    Description = "Award-winning fiction novel",
                    Price = 14.99m,
                    ImageUrl = "/images/novel.jpg",
                    CategoryId = booksId,
                    Category = categories[3],
                    IsFeatured = false,
                    CreatedAt = DateTime.UtcNow.AddDays(-20),
                    StockQuantity = 200
                }
            };
        }
    }
}