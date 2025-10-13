namespace ECommerce.Orders.Domain;

public record OrderItem(
    string ProductId,
    string ProductName,
    string ProductPicture,
    decimal Price,
    int Quantity)
{
    public decimal TotalUnit => Price * Quantity;
}
