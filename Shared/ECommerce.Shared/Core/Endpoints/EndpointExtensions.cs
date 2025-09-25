using Asp.Versioning;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;
using ECommerce.Shared.Core.Endpoints.Filters;
using Microsoft.OpenApi.Models;

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

    public static RouteHandlerBuilder SetEndpointConfiguration(this RouteHandlerBuilder builder, string summary = null,
        string description = null, string operationId = null, params string[] apiTag)
    {
        return builder
            .WithTags(apiTag)
            .WithOpenApi(operation => new OpenApiOperation(operation)
            {
                Summary = summary,
                Description = description,
                OperationId = operationId
            });
    }

    public static RouteHandlerBuilder WithRequestValidation<TRequest>(this RouteHandlerBuilder builder)
    {
        return builder
            .AddEndpointFilter<RequestValidationFilter<TRequest>>()
            .ProducesValidationProblem();
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