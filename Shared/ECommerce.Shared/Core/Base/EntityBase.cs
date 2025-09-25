namespace ECommerce.Shared.Core.Base;

public abstract class EntityBase(string id = "")
{
    public string Id { get; protected set; } = string.IsNullOrWhiteSpace(id)
        ? Guid.CreateVersion7().ToString() 
        : id;
}