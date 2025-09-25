using ECommerce.Customers.Domain.ValueObjects;
using ECommerce.Shared.Core.Base;

namespace ECommerce.Customers.Domain;

public class Customer : EntityBase
{    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public Address Address { get; set; }
    public IList<Provider> Providers { get; set; } = new List<Provider>();
}