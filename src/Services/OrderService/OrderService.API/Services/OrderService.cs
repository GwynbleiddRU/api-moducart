using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OrderService.API.Models;
using OrderService.API.Repositories;

namespace OrderService.API.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IOrderRepository orderRepository, ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public async Task<Order> CreateOrderAsync(Order order)
        {
            order.CreatedAt = DateTime.UtcNow;
            order.Status = "Pending"; // Default status
            return await _orderRepository.AddOrderAsync(order);
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId)
        {
            return await _orderRepository.GetOrdersByUserIdAsync(userId);
        }

        public async Task<Order> GetOrderByIdAsync(string orderId)
        {
            return await _orderRepository.GetOrderByIdAsync(orderId);
        }

        public async Task<bool> UpdateOrderStatusAsync(string orderId, string status)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null)
                return false;

            order.Status = status;
            await _orderRepository.UpdateOrderAsync(order);
            return true;
        }

        public async Task<bool> DeleteOrderAsync(string orderId)
        {
            return await _orderRepository.DeleteOrderAsync(orderId);
        }
    }
}
