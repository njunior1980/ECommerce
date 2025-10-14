using ECommerce.Shared.Core.Endpoints;
using ECommerce.Shared.Infrastructure.CQRS;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace ECommerce.Catalog.Endpoints;

public class CreateCategory : IEndpoint
{
    internal record Request(string Name);

    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost(Constants.ApiRoute.CreateCategory, async (Request request, IDispatcher dispatcher) =>
        {
            try
            {
                var command = new Commands.CreateCategoryCommand(request.Name);
                var result = await dispatcher.Send(command);

                return result.IsSuccess
                    ? Results.Created($"{Constants.ApiRoute.CreateCategory}/{result.Value}", null)
                    : Results.BadRequest(result.ErrorMessage);
            }
            catch (Exception e)
            {
                return Results.InternalServerError(e.Message);
            }
        })
        .WithRequestValidation<Request>()
        .SetEndpointConfiguration(
            summary: "Create a new category",
            description: "Creates a new category with the provided name.",
            operationId: nameof(CreateCategory),
            apiTag: [Constants.Tag],
            produces: [
                StatusCodes.Status200OK,
                StatusCodes.Status400BadRequest,
                StatusCodes.Status500InternalServerError
            ]);

    }
}