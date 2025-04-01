using System;
using System.ComponentModel.DataAnnotations;

namespace OrderService.API.DTOs
{
    public class PaymentRequestDto
    {
        [Required]
        public string OrderId { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        public string Currency { get; set; }

        [Required]
        public string PaymentMethod { get; set; }

        public string CardNumber { get; set; }
        public string CardExpiryMonth { get; set; }
        public string CardExpiryYear { get; set; }
        public string CardCvv { get; set; }

        public string BillingName { get; set; }
        public string BillingAddress { get; set; }
        public string BillingCity { get; set; }
        public string BillingState { get; set; }
        public string BillingZipCode { get; set; }
        public string BillingCountry { get; set; }
    }

    public class PaymentResponseDto
    {
        public string PaymentId { get; set; }
        public string OrderId { get; set; }
        public string Status { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public DateTime ProcessedAt { get; set; }
        public string TransactionReference { get; set; }
        public string Message { get; set; }
    }

    public class PaymentStatusDto
    {
        public string PaymentId { get; set; }
        public string OrderId { get; set; }
        public string Status { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public DateTime ProcessedAt { get; set; }
        public DateTime? LastUpdated { get; set; }
        public string PaymentMethod { get; set; }
        public string TransactionReference { get; set; }
    }
}
