using ECommerce.Shared.Core.Endpoints;
using ECommerce.Shared.Infrastructure.CQRS;
using ECommerce.Shared.Infrastructure.RavenDB;
using ECommerce.Shared.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Shared.Infrastructure;

public static class Registrar
{
    public static IServiceCollection AddAddressLookupService(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<IAddressLookupService, AddressLookupService>(client =>
        {            
            client.BaseAddress = new Uri(string.Empty);
        });

        return services;
    }

    public static IServiceCollection AddSharedInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var assembly = typeof(Registrar).Assembly;

        services.AddEndpoints(assembly);
        //services.AddFluentValidators();
        services.AddCQRS(assembly);
        services.AddRavenDB();

        return services;
    }
}