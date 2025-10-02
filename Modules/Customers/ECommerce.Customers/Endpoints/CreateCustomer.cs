using ECommerce.Customers.Commands;
using ECommerce.Shared.Core.Endpoints;
using ECommerce.Shared.Infrastructure.CQRS;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace ECommerce.Customers.Endpoints;

public class CreateCustomer : IEndpoint
{
    internal record Request(string Name, string Email);

    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost(Constants.ApiRoute.Create, async (Request request, IDispatcher dispatcher) =>
        {
            var command = new CreateCustomerCommand(request.Name, request.Email);
            
            var result = await dispatcher.Send(command);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : Results.BadRequest(result.Error);
        })
        .WithTags(Constants.Tag)
        .WithName(nameof(CreateCustomer))
        .WithDisplayName("Add a new customer")
        .Produces(200)
        .Produces(400);
    }    
}