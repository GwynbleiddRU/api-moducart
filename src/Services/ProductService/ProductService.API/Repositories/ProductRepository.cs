using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using ProductService.API.Data;
using ProductService.API.Models;

namespace ProductService.API.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _context.Products.Find(_ => true).ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(string id)
        {
            return await _context.Products.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category)
        {
            return await _context
                .Products.Find(p => p.Category.Name.ToLower() == category.ToLower())
                .ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetFeaturedProductsAsync()
        {
            return await _context.Products.Find(p => p.IsFeatured).Limit(4).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetNewArrivalsAsync()
        {
            return await _context
                .Products.Find(_ => true)
                .SortByDescending(p => p.CreatedAt)
                .Limit(3)
                .ToListAsync();
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            await _context.Products.InsertOneAsync(product);
            return product;
        }

        public async Task UpdateProductAsync(Product product)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Id, product.Id);
            await _context.Products.ReplaceOneAsync(filter, product);
        }

        public async Task DeleteProductAsync(string id)
        {
            var filter = Builders<Product>.Filter.Eq(p => p.Id, id);
            await _context.Products.DeleteOneAsync(filter);
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await _context.Categories.Find(_ => true).ToListAsync();
        }
    }
}
