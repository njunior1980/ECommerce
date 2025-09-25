namespace ECommerce.Customers.Domain.ValueObjects;

public record Address(
    string ZipCode, 
    string Street, 
    string City, 
    string State, 
    string Country, 
    string Complement = "");