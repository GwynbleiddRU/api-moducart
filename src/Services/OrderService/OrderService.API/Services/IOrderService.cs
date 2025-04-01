using OrderService.API.DTOs;
using OrderService.API.Models;

namespace OrderService.API.Services
{
    public interface IOrdersService
    {
        Task<Order> CreateOrderAsync(string userId, CreateOrderDto orderDto);
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId);
        Task<Order> GetOrderByIdAsync(string orderId);
        Task<bool> UpdateOrderStatusAsync(string orderId, string status);
        Task<bool> DeleteOrderAsync(string orderId);
    }
}
