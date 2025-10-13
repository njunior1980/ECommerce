using ECommerce.Orders.Domain.ValueObjects;
using ECommerce.Shared.Core.Base;

namespace ECommerce.Orders.Domain;

public class Order : EntityBase
{
    public string OrderNumber { get; } = GenerateOrderNumber();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string CustomerId { get; set; }
    public string CustomerName { get; set; }
    public string CustomerPhone { get; set; }
    public string CustomerEmail { get; set; }
    public IList<OrderItem> Items { get; set; } = new List<OrderItem>();
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public string StatusMessage { get; set; }
    public Address ShippingAddress { get; set; }

    public void AddItem(OrderItem item)
    {
        Items.Add(item);
        TotalAmount = Items.Sum(i => i.TotalUnit);
    }

    public void UpdateStatus(OrderStatus status)
    {
        Status = status;
    }

    private static string GenerateOrderNumber()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var guidBytes = Guid.NewGuid().ToByteArray();

        var number = BitConverter.ToInt64(guidBytes, 0);
        number = Math.Abs(number);

        var result = "";
        while (number > 0 && result.Length < 8)
        {
            result = chars[(int)(number % chars.Length)] + result;
            number /= chars.Length;
        }

        return $"ORD-{result.PadLeft(8, '0')}";
    }
}