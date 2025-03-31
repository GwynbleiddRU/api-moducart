using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using OrderService.API.Models;

namespace OrderService.API.Repositories
{
    public interface IPaymentsRepository
    {
        Task<Payment> CreatePaymentAsync(Payment payment);
        Task<Payment> GetPaymentByIdAsync(string id);
        Task<IEnumerable<Payment>> GetPaymentsByUserIdAsync(string userId);
        Task UpdatePaymentStatusAsync(string id, string status);
    }
}
