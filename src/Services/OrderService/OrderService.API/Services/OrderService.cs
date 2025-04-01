using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OrderService.API.DTOs;
using OrderService.API.Models;
using OrderService.API.Repositories;

namespace OrderService.API.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ILogger<OrdersService> _logger;

        public OrdersService(IOrderRepository orderRepository, ILogger<OrdersService> logger)
        {
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public async Task<Order> CreateOrderAsync(string userId, CreateOrderDto createOrderDto)
        {
            var order = new Order
            {
                UserId = userId,
                Items = createOrderDto
                    .Items.Select(item => new OrderItem
                    {
                        ProductId = item.ProductId,
                        ProductName = item.ProductName,
                        Price = item.Price,
                        Quantity = item.Quantity,
                        ImageUrl = item.ImageUrl,
                    })
                    .ToList(),
                TotalPrice = createOrderDto.Items.Sum(i => i.Price * i.Quantity),
                Status = "Pending",
                CreatedAt = DateTime.UtcNow,
                ShippingAddress = new ShippingAddress
                {
                    FullName = createOrderDto.ShippingAddress.FullName,
                    AddressLine1 = createOrderDto.ShippingAddress.AddressLine1,
                    AddressLine2 = createOrderDto.ShippingAddress.AddressLine2,
                    City = createOrderDto.ShippingAddress.City,
                    State = createOrderDto.ShippingAddress.State,
                    ZipCode = createOrderDto.ShippingAddress.ZipCode,
                    Country = createOrderDto.ShippingAddress.Country,
                    PhoneNumber = createOrderDto.ShippingAddress.PhoneNumber,
                },
                PaymentInfo = new PaymentInfo
                {
                    PaymentMethod = createOrderDto.PaymentInfo.PaymentMethod,
                    PaymentId = createOrderDto.PaymentInfo.PaymentId,
                    TransactionId = createOrderDto.PaymentInfo.TransactionId,
                },
            };

            // Save the order to the repository
            var createdOrder = await _orderRepository.CreateOrderAsync(order);
            return createdOrder;
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
            await _orderRepository.UpdateOrderStatusAsync(orderId, status);
            return true;
        }

        public async Task<bool> DeleteOrderAsync(string orderId)
        {
            return await _orderRepository.DeleteOrderAsync(orderId);
        }
    }
}
