using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OrderService.API.Models;

namespace OrderService.API.Repositories
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId);
        Task<Order> GetOrderByIdAsync(string orderId);
        Task<Order> AddOrderAsync(Order order);
        Task<bool> UpdateOrderAsync(Order order);
        Task<bool> DeleteOrderAsync(string orderId);
    }
}
