using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using ProductService.API.Data.Mocks;
using ProductService.API.Extensions;
using ProductService.API.Models;
using ProductService.API.Settings;

namespace ProductService.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IMongoCollection<Product> _products;
        private readonly IMongoCollection<Category> _categories;
        private readonly ILogger<ProductRepository> _logger;
        private bool _isConnected = true;

        public ProductRepository(IMongoDatabase database, ILogger<ProductRepository> logger)
        {
            _logger = logger;

            try
            {
                _products = database.GetCollection<Product>("Products");
                _categories = database.GetCollection<Category>("Categories");

                // Test connection
                database.RunCommandAsync((Command<BsonDocument>)"{ping:1}").Wait();
                _logger.LogInformation("MongoDB connection successful");
            }
            catch (Exception ex)
            {
                _isConnected = false;
                _logger.LogError(ex, "MongoDB connection failed. Using mock data.");
            }
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            if (!_isConnected)
            {
                _logger.LogWarning("Using mock data for GetAllProductsAsync");
                return MockProductData.GetMockProducts();
            }

            try
            {
                var products = await _products.Find(_ => true).ToListAsync();
                await PopulateCategoryInfoAsync(products);
                return products;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving products from MongoDB");
                return MockProductData.GetMockProducts();
            }
        }

        public async Task<Product> GetProductByIdAsync(string id)
        {
            if (!_isConnected)
            {
                _logger.LogWarning("Using mock data for GetProductByIdAsync");
                return MockProductData.GetMockProducts().FirstOrDefault(p => p.Id == id);
            }

            try
            {
                var product = await _products.Find(p => p.Id == id).FirstOrDefaultAsync();
                if (product != null)
                {
                    await PopulateCategoryInfoAsync(new List<Product> { product });
                }
                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving product {id} from MongoDB");
                return MockProductData.GetMockProducts().FirstOrDefault(p => p.Id == id);
            }
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category)
        {
            if (!_isConnected)
            {
                _logger.LogWarning("Using mock data for GetProductsByCategoryAsync");
                return MockProductData
                    .GetMockProducts()
                    .Where(p => p.Category?.Name.ToLower() == category.ToLower());
            }

            try
            {
                var categoryObj = await _categories
                    .Find(c => c.Name.ToLower() == category.ToLower())
                    .FirstOrDefaultAsync();

                if (categoryObj == null)
                    return new List<Product>();

                var products = await _products
                    .Find(p => p.CategoryId == categoryObj.Id)
                    .ToListAsync();

                await PopulateCategoryInfoAsync(products);
                return products;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    $"Error retrieving products for category {category} from MongoDB"
                );
                return MockProductData
                    .GetMockProducts()
                    .Where(p => p.Category?.Name.ToLower() == category.ToLower());
            }
        }

        public async Task<IEnumerable<Product>> GetFeaturedProductsAsync()
        {
            if (!_isConnected)
            {
                _logger.LogWarning("Using mock data for GetFeaturedProductsAsync");
                return MockProductData.GetMockProducts().Where(p => p.IsFeatured).Take(4);
            }

            try
            {
                var products = await _products.Find(p => p.IsFeatured).Limit(4).ToListAsync();

                await PopulateCategoryInfoAsync(products);
                return products;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving featured products from MongoDB");
                return MockProductData.GetMockProducts().Where(p => p.IsFeatured).Take(4);
            }
        }

        public async Task<IEnumerable<Product>> GetNewArrivalsAsync()
        {
            if (!_isConnected)
            {
                _logger.LogWarning("Using mock data for GetNewArrivalsAsync");
                return MockProductData
                    .GetMockProducts()
                    .OrderByDescending(p => p.CreatedAt)
                    .Take(3);
            }

            try
            {
                var products = await _products
                    .Find(_ => true)
                    .Sort(Builders<Product>.Sort.Descending(p => p.CreatedAt))
                    .Limit(3)
                    .ToListAsync();

                await PopulateCategoryInfoAsync(products);
                return products;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving new arrivals from MongoDB");
                return MockProductData
                    .GetMockProducts()
                    .OrderByDescending(p => p.CreatedAt)
                    .Take(3);
            }
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            if (!_isConnected)
            {
                _logger.LogWarning("Using mock data for GetAllCategoriesAsync");
                return MockProductData.GetMockCategories();
            }

            try
            {
                return await _categories.Find(_ => true).ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving categories from MongoDB");
                return MockProductData.GetMockCategories();
            }
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            if (!_isConnected)
            {
                _logger.LogWarning("MongoDB not connected. Cannot add product.");
                throw new InvalidOperationException("Cannot add product: Database not connected");
            }

            try
            {
                await _products.InsertOneAsync(product);
                return product;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding product to MongoDB");
                throw;
            }
        }

        public async Task UpdateProductAsync(Product product)
        {
            if (!_isConnected)
            {
                _logger.LogWarning("MongoDB not connected. Cannot update product.");
                throw new InvalidOperationException(
                    "Cannot update product: Database not connected"
                );
            }

            try
            {
                var filter = Builders<Product>.Filter.Eq(p => p.Id, product.Id);
                await _products.ReplaceOneAsync(filter, product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating product {product.Id} in MongoDB");
                throw;
            }
        }

        public async Task DeleteProductAsync(string id)
        {
            if (!_isConnected)
            {
                _logger.LogWarning("MongoDB not connected. Cannot delete product.");
                throw new InvalidOperationException(
                    "Cannot delete product: Database not connected"
                );
            }

            try
            {
                await _products.DeleteOneAsync(p => p.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting product {id} from MongoDB");
                throw;
            }
        }

        private async Task PopulateCategoryInfoAsync(IEnumerable<Product> products)
        {
            if (!products.Any())
                return;

            var categoryIds = products.Select(p => p.CategoryId).Distinct().ToList();
            var categories = new Dictionary<string, Category>();

            if (_isConnected)
            {
                try
                {
                    var filter = Builders<Category>.Filter.In(c => c.Id, categoryIds);
                    var categoriesFromDb = await _categories.Find(filter).ToListAsync();
                    categories = categoriesFromDb.ToDictionary(c => c.Id);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error retrieving categories for products");
                    var mockCategories = MockProductData.GetMockCategories();
                    categories = mockCategories.ToDictionary(c => c.Id);
                }
            }
            else
            {
                var mockCategories = MockProductData.GetMockCategories();
                categories = mockCategories.ToDictionary(c => c.Id);
            }

            foreach (var product in products)
            {
                if (categories.TryGetValue(product.CategoryId, out var category))
                {
                    product.Category = category;
                }
            }
        }
    }
}
