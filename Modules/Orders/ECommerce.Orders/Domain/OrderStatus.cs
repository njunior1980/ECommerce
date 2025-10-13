namespace ECommerce.Orders.Domain;

public enum OrderStatus
{
    Pending,
    Processing,
    Shipped,
    Delivered,
    Canceled
}