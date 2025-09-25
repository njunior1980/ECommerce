using Microsoft.Extensions.Logging;

namespace ECommerce.Shared.Infrastructure.Services;

public record AddressLookupResponse(
    string ZipCode,
    string Street,
    string City,
    string State,
    string Country
);

public interface IAddressLookupService
{
    Task<AddressLookupResponse> LookupAddress(string zipCode, CancellationToken cancellationToken = default);
}

public class AddressLookupService(HttpClient httpClient, ILogger<AddressLookupService> logger) : IAddressLookupService
{
    public async Task<AddressLookupResponse> LookupAddress(string zipCode, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var response = await httpClient.GetAsync($"https://api.example.com/lookup?zip={zipCode}", cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            logger.LogError("Failed to lookup address for zip code {ZipCode}", zipCode);
            throw new Exception("Failed to lookup address");
        }

        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        
        return new AddressLookupResponse(zipCode, "Street", "City", "State", "Country");
    }
}