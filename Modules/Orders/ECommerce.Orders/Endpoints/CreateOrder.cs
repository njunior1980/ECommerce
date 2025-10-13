using ECommerce.Orders.Commands;
using ECommerce.Orders.Domain;
using ECommerce.Orders.Domain.ValueObjects;
using ECommerce.Shared.Core.Endpoints;
using ECommerce.Shared.Infrastructure.CQRS;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace ECommerce.Orders.Endpoints;

public class CreateOrder : IEndpoint
{
    internal record Request(
        string CustomerId,
        string CustomerName,
        string CustomerEmail,
        Address ShippingAddress,
        IList<OrderItem> Items);

    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost(Constants.ApiRoute.Create, async (Request request, IDispatcher dispatcher, CancellationToken ct = default) =>
        {
            try
            {
                if (ct.IsCancellationRequested)
                    return Results.BadRequest(new { Message = Constants.Errors.RequestCancelled });

                var command = new CreateOrderCommand(
                    request.CustomerId,
                    request.CustomerName,
                    request.CustomerEmail,
                    request.ShippingAddress,
                    request.Items);

                var result = await dispatcher.Send(command, ct);

                return result.IsSuccess
                    ? Results.Ok(new { Message = Constants.Messages.OrderCreated, OrderId = result.Value })
                    : Results.BadRequest(result.ErrorMessage);
            }
            catch (Exception e)
            {
                return Results.InternalServerError(e.Message);
            }
        })
        .WithRequestValidation<Request>()
        .SetEndpointConfiguration(
            summary: "Create a new order",
            description: "Creates a new order with the provided customer and order details.",
            operationId: nameof(CreateOrder),
            apiTag: [Constants.Tag],
            produces: [
                StatusCodes.Status200OK,
                StatusCodes.Status400BadRequest,
                StatusCodes.Status500InternalServerError
            ]);
    }

    internal class CreateOrderValidation : AbstractValidator<Request>
    {
        public CreateOrderValidation()
        {
            RuleFor(x => x.CustomerId).NotEmpty().WithMessage("CustomerId is required.");
            RuleFor(x => x.CustomerName).NotEmpty().WithMessage("CustomerName is required.");
            RuleFor(x => x.CustomerEmail).NotEmpty().EmailAddress().WithMessage("A valid CustomerEmail is required.");
            RuleFor(x => x.ShippingAddress).NotNull().WithMessage("ShippingAddress is required.");
            RuleFor(x => x.Items).NotEmpty().WithMessage("At least one order item is required.");
        }
    }
}