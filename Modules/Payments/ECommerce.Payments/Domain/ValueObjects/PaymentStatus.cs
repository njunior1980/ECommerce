namespace ECommerce.Payments.Domain.ValueObjects;

public enum PaymentStatus
{
    Pending,
    Completed,
    Failed,
    Refunded
}