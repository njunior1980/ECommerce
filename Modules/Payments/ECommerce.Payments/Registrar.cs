using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Payments;

public static class Registrar
{
    public static IServiceCollection AddPaymentsModule(this IServiceCollection services)
    {
        // Register module-specific services here
        return services;
    }
}