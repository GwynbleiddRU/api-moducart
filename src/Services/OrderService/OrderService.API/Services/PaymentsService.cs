using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PaymentService.API.DTOs;
using PaymentService.API.Models;
using PaymentService.API.Repositories;

namespace OrderService.API.Services
{
    public class PaymentsService : IPaymentsService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly ILogger<PaymentsService> _logger;

        public PaymentsService(
            IPaymentRepository paymentRepository,
            ILogger<PaymentsService> logger
        )
        {
            _paymentRepository = paymentRepository;
            _logger = logger;
        }

        public async Task<PaymentResponseDto> ProcessPaymentAsync(PaymentRequestDto request)
        {
            try
            {
                var payment = new Payment
                {
                    Id = Guid.NewGuid().ToString(),
                    OrderId = request.OrderId,
                    UserId = request.UserId,
                    Amount = request.Amount,
                    PaymentMethod = request.PaymentMethod,
                    Status = "Processing",
                    CreatedAt = DateTime.UtcNow,
                };

                await _paymentRepository.AddPaymentAsync(payment);

                // Simulating external payment processing
                payment.Status = "Completed";
                await _paymentRepository.UpdatePaymentStatusAsync(payment.Id, payment.Status);

                return new PaymentResponseDto
                {
                    PaymentId = payment.Id,
                    Status = payment.Status,
                    ProcessedAt = payment.CreatedAt,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Error processing payment for Order {OrderId}",
                    request.OrderId
                );
                throw new InvalidOperationException("Payment processing failed.");
            }
        }

        public async Task<PaymentStatusDto> GetPaymentStatusAsync(string paymentId)
        {
            var payment = await _paymentRepository.GetPaymentByIdAsync(paymentId);
            if (payment == null)
                throw new KeyNotFoundException("Payment not found.");

            return new PaymentStatusDto { PaymentId = payment.Id, Status = payment.Status };
        }
    }
}
