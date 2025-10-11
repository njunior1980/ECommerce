using Asp.Versioning;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace ECommerce.Shared.Core.Endpoints;

public static class EndpointExtensions
{
    public static void AddEndpoints(this IServiceCollection services, Assembly assembly)
    {
        var serviceDescriptors = assembly.DefinedTypes
            .Where(type => type is { IsAbstract: false, IsInterface: false } &&
                           type.IsAssignableTo(typeof(IEndpoint)))
            .Select(type => ServiceDescriptor.Transient(typeof(IEndpoint), type))
            .ToArray();

        services.TryAddEnumerable(serviceDescriptors);
    }   

    public static IApplicationBuilder UseMapEndpoints(this WebApplication app)
    {
        var versionSet = app.NewApiVersionSet()
            .HasApiVersion(ApiVersion.Default)
            .ReportApiVersions()
            .Build();

        app.MapGroup("api/v{version:apiVersion}")
            .WithApiVersionSet(versionSet);

        var endpoints = app.Services.GetRequiredService<IEnumerable<IEndpoint>>().ToList();
        endpoints.ForEach(p => p.MapEndpoint(app));

        return app;
    }
}