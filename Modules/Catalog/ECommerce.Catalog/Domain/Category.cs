using ECommerce.Shared.Core.Base;

namespace ECommerce.Catalog.Domain;

internal class Category : EntityBase
{
    public string Name { get; private set; }

    public static Category Create(string name)
    {
        return new Category { Name = name };
    }

    public void Update(string name)
    {
        Name = name;
    }
}