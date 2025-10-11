using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace ECommerce.Shared.Core.Endpoints;

public static class RouteHandlerBuilderExtensions
{
    public static RouteHandlerBuilder WithRequestValidation<TRequest>(this RouteHandlerBuilder builder)
    {
        return builder
            .AddEndpointFilter<RequestValidationFilter<TRequest>>()
            .ProducesValidationProblem();
    }

    public static RouteHandlerBuilder Produces(this RouteHandlerBuilder builder, params int[] statusCodes)
    {
        return statusCodes.Aggregate(builder, (current, code) => current.Produces(code));
    }

    private static RouteHandlerBuilder Produces(this RouteHandlerBuilder builder,
        params (int StatusCode, Type ResponseType)[] responses)
    {
        foreach (var (code, type) in responses)
        {
            var method = typeof(RouteHandlerBuilderExtensions)
                .GetMethod(nameof(ApplyProducesGeneric), BindingFlags.NonPublic | BindingFlags.Static)!
                .MakeGenericMethod(type);

            builder = (RouteHandlerBuilder)method.Invoke(null, [builder, code])!;
        }

        return builder;
    }

    public static RouteHandlerBuilder Produces<TResponse>(this RouteHandlerBuilder builder,
        params int[] statusCodes)
    {
        return statusCodes.Aggregate(builder, (current, code) => current.Produces<TResponse>(code));
    }

    private static RouteHandlerBuilder ApplyProducesGeneric<T>(
        RouteHandlerBuilder builder, int statusCode)
        => builder.Produces<T>(statusCode);

    public static RouteHandlerBuilder SetEndpointConfiguration(
        this RouteHandlerBuilder builder,
        string summary,
        string description,
        string operationId,
        string[] apiTag,
        object[] produces = null)
    {
        builder.WithSummary(summary)
            .WithDescription(description)
            .WithTags(apiTag)
            .WithOpenApi(operation =>
            {
                operation.OperationId = operationId;
                operation.Summary = summary;
                operation.Description = description;
                return operation;
            });

        if (produces is not { Length: > 0 })
            return builder;

        var typed = produces.OfType<(int, Type)>().ToArray();
        var untyped = produces.OfType<int>().ToArray();

        if (typed.Length > 0)
            builder = builder.Produces(typed);
        if (untyped.Length > 0)
            builder = builder.Produces(untyped);

        return builder;
    }
}