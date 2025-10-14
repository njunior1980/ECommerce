using ECommerce.Customers.Domain.ValueObjects;
using ECommerce.Shared.Core.Base;

namespace ECommerce.Customers.Domain;

public class Customer : EntityBase
{
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }
    public string Phone { get; private set; }
    public Address ShippingAddress { get; private set; }
    public IList<Provider> Providers { get; } = new List<Provider>();

    public static Customer Create(string name, string email, string phone)
    {
        return new Customer
        {
            Name = name,
            Email = email,
            Phone = phone,
            CreatedAt = DateTime.UtcNow
        };
    }

    public void AddShippingAddress(Address address)
    {
        ShippingAddress = address;
        UpdatedAt = DateTime.UtcNow;
    }

    public void Update(string name, string email, string phone, Address shippingAddress = null)
    {
        Name = name;
        Email = email;
        Phone = phone;

        if (shippingAddress is not null)
            ShippingAddress = shippingAddress;

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