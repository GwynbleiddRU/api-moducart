using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OrderService.API.Models
{
    public class Order
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

        [BsonElement("userId")]
        public required string UserId { get; set; }

        [BsonElement("items")]
        public required List<OrderItem> Items { get; set; }

        [BsonElement("totalPrice")]
        public decimal TotalPrice { get; set; }

        [BsonElement("status")]
        public string Status { get; set; } = "Pending";

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("shippingAddress")]
        public required ShippingAddress ShippingAddress { get; set; }

        [BsonElement("paymentInfo")]
        public required PaymentInfo PaymentInfo { get; set; }
    }

    public class OrderItem
    {
        [BsonElement("productId")]
        public required string ProductId { get; set; }

        [BsonElement("productName")]
        public required string ProductName { get; set; }

        [BsonElement("imageUrl")]
        public required string ImageUrl { get; set; }

        [BsonElement("quantity")]
        public int Quantity { get; set; }

        [BsonElement("price")]
        public decimal Price { get; set; }
    }

    public class ShippingAddress
    {
        public required string FullName { get; set; }
        public required string AddressLine1 { get; set; }
        public required string AddressLine2 { get; set; }
        public required string City { get; set; }
        public required string State { get; set; }
        public required string ZipCode { get; set; }
        public required string Country { get; set; }
        public required string PhoneNumber { get; set; }
    }
}
