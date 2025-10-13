using ECommerce.Orders.Domain;
using ECommerce.Orders.Queries;
using ECommerce.Shared.Core.Endpoints;
using ECommerce.Shared.Infrastructure.CQRS;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace ECommerce.Orders.Endpoints;

public class ListOrders : IEndpoint
{
    internal class RequestFilter
    {
        public DateTime? CreatedAtFrom { get; set; }
        public DateTime? CreatedAtTo { get; set; }
        public OrderStatus Status { get; set; }
    }

    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapGet(Constants.ApiRoute.List, async (string customerId, RequestFilter filter, IDispatcher dispatcher, CancellationToken ct) =>
        {
            try
            {
                var query = new ListOrdersQuery(customerId, filter.CreatedAtFrom, filter.CreatedAtTo, filter.Status);

                var result = await dispatcher.Send(query, ct);

                return result.IsSuccess
                    ? Results.Ok(result.Value)
                    : Results.BadRequest(result.ErrorMessage);
            }
            catch (Exception e)
            {
                return Results.InternalServerError(e.Message);
            }
        })
        .SetEndpointConfiguration(
            summary: "List all orders",
            description: "Retrieves a list of all orders in the system.",
            operationId: nameof(ListOrders),
            apiTag: [Constants.Tag],
            produces: [
                StatusCodes.Status200OK,
                StatusCodes.Status500InternalServerError
            ]);
    }
}