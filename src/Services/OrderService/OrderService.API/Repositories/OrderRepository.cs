using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using OrderService.API.Models;
using OrderService.API.Settings;

namespace OrderService.API.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IMongoCollection<Order> _orders;

        public OrderRepository(IMongoDatabase database)
        {
            _orders = database.GetCollection<Order>("Orders");
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId)
        {
            return await _orders.Find(o => o.UserId == userId).ToListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(string orderId)
        {
            return await _orders.Find(o => o.Id == orderId).FirstOrDefaultAsync();
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            order.CreatedAt = DateTime.UtcNow;
            order.Status = "Pending";
            await _orders.InsertOneAsync(order);
            return order;
        }

        public async Task<bool> UpdateOrderStatusAsync(string orderId, string status)
        {
            var update = Builders<Order>.Update.Set(o => o.Status, status);
            var result = await _orders.UpdateOneAsync(o => o.Id == orderId, update);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteOrderAsync(string orderId)
        {
            var result = await _orders.DeleteOneAsync(o => o.Id == orderId);
            return result.DeletedCount > 0;
        }
    }
}
