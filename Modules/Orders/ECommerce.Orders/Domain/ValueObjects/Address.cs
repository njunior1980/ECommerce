namespace ECommerce.Orders.Domain.ValueObjects;

public record Address
{
    public required string ZipCode { get; init; }
    public required string Street { get; init; }
    public required string City { get; init; }
    public required string State { get; init; }
    public required string Country { get; init; }
    public string Complement { get; set; }
}