using ECommerce.Shared.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Shared.Infrastructure;

public static class RegisterModule
{
    public static IServiceCollection AddAddressLookupService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<IAddressLookupService, AddressLookupService>(client =>
        {            
            client.BaseAddress = new Uri(string.Empty);
        });

        return services;
    }
}