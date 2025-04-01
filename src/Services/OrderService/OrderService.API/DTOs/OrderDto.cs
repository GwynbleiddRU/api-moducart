using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OrderService.API.DTOs
{
    public class OrderDto
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderItemDto> Items { get; set; }
        public ShippingAddressDto ShippingAddress { get; set; }
        public PaymentInfoDto PaymentInfo { get; set; }
    }

    public class CreateOrderDto
    {
        [Required]
        public List<OrderItemDto> Items { get; set; }

        [Required]
        public ShippingAddressDto ShippingAddress { get; set; }

        [Required]
        public PaymentInfoDto PaymentInfo { get; set; }
    }

    public class OrderItemDto
    {
        [Required]
        public string ProductId { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
        public int Quantity { get; set; }

        public string ImageUrl { get; set; }
    }

    public class ShippingAddressDto
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public string ZipCode { get; set; }

        [Required]
        public string Country { get; set; }

        public string PhoneNumber { get; set; }
    }

    public class PaymentInfoDto
    {
        [Required]
        public string PaymentMethod { get; set; }

        public string PaymentId { get; set; }

        public string TransactionId { get; set; }
    }
}
