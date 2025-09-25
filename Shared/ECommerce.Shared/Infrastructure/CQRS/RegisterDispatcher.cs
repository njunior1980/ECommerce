using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Shared.Infrastructure.CQRS;

public static class RegisterDispatcher
{
    public static void AddCQRS(this IServiceCollection services, Assembly assembly)
    {
        var handlerInterfaces = new[]
        {
            typeof(ICommandHandler<,>),
            typeof(IQueryHandler<,>)
        };

        var types = assembly.GetTypes()
            .SelectMany(implementationType => implementationType.GetInterfaces()
                .Where(p => p.IsGenericType && handlerInterfaces.Contains(p.GetGenericTypeDefinition()))
                .Select(serviceType => new
                {
                    Interface = serviceType,
                    Implementation = implementationType
                }));

        foreach (var type in types)
            services.AddTransient(type.Interface, type.Implementation);

        services.AddSingleton<IDispatcher, Dispatcher>();
    }
}