namespace OrderService.API.Services
{
    public interface IOrderService
    {
        Task<Order> CreateOrderAsync(Order order);
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId);
        Task<Order> GetOrderByIdAsync(string orderId);
        Task<bool> UpdateOrderStatusAsync(string orderId, string status);
        Task<bool> DeleteOrderAsync(string orderId);
    }
}
