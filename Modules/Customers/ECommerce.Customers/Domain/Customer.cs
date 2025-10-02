using ECommerce.Customers.Domain.ValueObjects;
using ECommerce.Shared.Core.Base;

namespace ECommerce.Customers.Domain;

public class Customer : EntityBase
{    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public Address ShipTo { get; set; }
    public IList<Provider> Providers { get; set; } = new List<Provider>();

    public void Create(string name, string email)
    {
        Name = name;
        Email = email;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string name, string email)
    {
        Name = name;
        Email = email;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddOrUpdateAddress(Address address)
    {
        ShipTo = address;
        UpdatedAt = DateTime.UtcNow;
    }

    public void AddProvider(Provider provider)
    {
        if (Providers.Any(p => p.Service == provider.Service && p.AccountId == provider.AccountId))
        {
            return;
        }

        Providers.Add(provider);
        UpdatedAt = DateTime.UtcNow;
    }
}