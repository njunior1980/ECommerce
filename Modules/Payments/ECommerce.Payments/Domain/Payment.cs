using ECommerce.Payments.Domain.ValueObjects;
using ECommerce.Shared.Core.Base;

namespace ECommerce.Payments.Domain;

public class Payment : EntityBase
{
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string OrderId { get; set; }
    public string CustomerId { get; set; }
    public string CustomerName { get; set; }
    public string CustomerEmail { get; set; }
    public string CustomerPhone { get; set; }
    public string StripeUserId { get; set; }
    public PaymentMethod Method { get; set; }
    public string PaymentCode { get; set; }
    public decimal Amount { get; set; }
    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;
    public string StatusMessage { get; set; }
}