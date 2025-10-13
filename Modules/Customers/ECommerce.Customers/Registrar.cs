using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Customers;

public static class Registrar
{
    public static IServiceCollection AddCustomersModule(this IServiceCollection services)
    {
        // Register module-specific services here
        return services;
    }
}