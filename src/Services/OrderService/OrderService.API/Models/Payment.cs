using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OrderService.API.Models
{
    public class Payment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public required string Id { get; set; }

        [BsonElement("userId")]
        public required string UserId { get; set; }

        [BsonElement("orderId")]
        public required string OrderId { get; set; }

        [BsonElement("paymentMethod")]
        public required string PaymentMethod { get; set; }

        [BsonElement("amount")]
        public decimal Amount { get; set; }

        [BsonElement("currency")]
        public required string Currency { get; set; }

        [BsonElement("status")]
        public required string Status { get; set; } // Pending, Completed, Failed

        [BsonElement("createdAt")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [BsonElement("updatedAt")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }

    public class PaymentInfo
    {
        [BsonElement("paymentMethod")]
        public required string PaymentMethod { get; set; }

        [BsonElement("paymentId")]
        public required string PaymentId { get; set; }

        [BsonElement("transactionId")]
        public required string TransactionId { get; set; }
    }
}
