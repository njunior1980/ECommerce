using ECommerce.Payments.Commands;
using ECommerce.Shared.Core.Endpoints;
using ECommerce.Shared.Infrastructure.CQRS;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace ECommerce.Payments.Endpoints;

public class CreatePayment : IEndpoint
{
    internal record Request(
        string OrderId,
        string CustomerId,
        string CustomerName,
        string CustomerEmail,
        string CustomerPhone,
        decimal Amount);

    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost(Constants.ApiRoute.Create, async (Request request, IDispatcher dispatcher) =>
        {
            try
            {
                var cmd = new CreatePaymentCommand(request.OrderId, request.CustomerId, request.CustomerName,
                    request.CustomerEmail, request.CustomerPhone, request.Amount);

                var result = await dispatcher.Send(cmd);

                return result.IsSuccess
                    ? Results.Ok(result.Value)
                    : Results.BadRequest(result.Error);
            }
            catch (Exception e)
            {
                return Results.InternalServerError(e.Message);
            }
        })
            .WithRequestValidation<Request>()
            .SetEndpointConfiguration(
            summary: "Create a new payment",
            description: "Creates a new payment with the provided payment details.",
            operationId: nameof(CreatePayment),
            apiTag: [Constants.Tag],
            produces: [
                StatusCodes.Status200OK,
                StatusCodes.Status400BadRequest,
                StatusCodes.Status500InternalServerError
            ]);
    }

    internal class CreatePaymentValidation : AbstractValidator<Request>
    {
        public CreatePaymentValidation()
        {
            RuleFor(x => x.OrderId).NotEmpty().WithMessage("OrderId is required.");
            RuleFor(x => x.CustomerId).NotEmpty().WithMessage("CustomerId is required.");
            RuleFor(x => x.CustomerName).NotEmpty().WithMessage("CustomerName is required.");
            RuleFor(x => x.CustomerEmail).NotEmpty().EmailAddress().WithMessage("A valid CustomerEmail is required.");
            RuleFor(x => x.Amount).GreaterThan(0).WithMessage("Amount must be greater than zero.");
        }
    }
}