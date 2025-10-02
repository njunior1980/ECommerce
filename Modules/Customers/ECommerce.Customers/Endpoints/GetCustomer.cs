using ECommerce.Customers.Queries;
using ECommerce.Shared.Core.Endpoints;
using ECommerce.Shared.Infrastructure.CQRS;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace ECommerce.Customers.Endpoints;

public class GetCustomer : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapGet(Constants.ApiRoute.Get, async (string id, IDispatcher dispatcher) =>
        {
            var result = await dispatcher.Send(new GetCustomerQuery(id));

            return result.IsSuccess
                ? Results.NotFound(result.ErrorMessage)
                : Results.Ok(result.Value);

        })
        .WithTags(Constants.Tag)
        .WithName(nameof(GetCustomer))
        .WithDisplayName("Get a customer by ID")
        .Produces(200)
        .Produces(404);
    }    
}