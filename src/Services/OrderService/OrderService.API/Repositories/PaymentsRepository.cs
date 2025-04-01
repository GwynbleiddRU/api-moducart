using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using OrderService.API.Models;

// using OrderService.API.Settings;

namespace OrderService.API.Repositories
{
    public class PaymentsRepository : IPaymentsRepository
    {
        private readonly IMongoCollection<Payment> _paymentsCollection;

        public PaymentsRepository(IMongoDatabase database)
        {
            _paymentsCollection = database.GetCollection<Payment>("Payments");
        }

        public async Task<Payment> CreatePaymentAsync(Payment payment)
        {
            await _paymentsCollection.InsertOneAsync(payment);
            return payment;
        }

        public async Task<Payment> GetPaymentByIdAsync(string id)
        {
            return await _paymentsCollection.Find(p => p.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByUserIdAsync(string userId)
        {
            return await _paymentsCollection.Find(p => p.UserId == userId).ToListAsync();
        }

        public async Task UpdatePaymentStatusAsync(string id, string status)
        {
            var update = Builders<Payment>.Update.Set(p => p.Status, status);
            await _paymentsCollection.UpdateOneAsync(p => p.Id == id, update);
        }
    }
}
