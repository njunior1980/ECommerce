using Microsoft.Extensions.DependencyInjection;

namespace ECommerce.Shared.Core.Events;

public static class RegisterDomainEvent
{
    public static void RegisterDomainEvents(this IServiceCollection services)
    {
        services.Scan(scan => scan.FromAssembliesOf(typeof(RegisterDomainEvent))
            .AddClasses(classes => classes.AssignableTo(typeof(IDomainEventHandler<>)), publicOnly: false)
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        services.AddTransient<IDomainEventsDispatcher, DomainEventsDispatcher>();
    }
}