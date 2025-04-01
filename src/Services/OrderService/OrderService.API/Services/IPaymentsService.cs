using System.Threading.Tasks;
using OrderService.API.DTOs;

namespace OrderService.API.Services
{
    public interface IPaymentsService
    {
        Task<PaymentResponseDto> ProcessPaymentAsync(PaymentRequestDto paymentRequest);
        Task<PaymentStatusDto> GetPaymentStatusAsync(string paymentId);
    }
}
