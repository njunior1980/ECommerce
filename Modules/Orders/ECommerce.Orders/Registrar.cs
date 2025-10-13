using ECommerce.Shared.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Orders;

public static class Registrar
{
    public static IServiceCollection AddOrdersModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSharedInfrastructure(configuration);        

        // Register module-specific services here
        return services;
    }
}