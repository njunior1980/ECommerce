using ECommerce.Customers.Commands;
using ECommerce.Shared.Core.Endpoints;
using ECommerce.Shared.Infrastructure.CQRS;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace ECommerce.Customers.Endpoints;

public class UpdateCustomer : IEndpoint
{
    internal record Request(string Id, string Name, string Email);

    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost(Constants.ApiRoute.Update, async (string id, Request request, IDispatcher dispatcher, CancellationToken ct) =>
        {
            try
            {
                var command = new UpdateCustomerCommand(id, request.Name, request.Email);

                var result = await dispatcher.Send(command, ct);

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
        summary: "Update an existing customer",
        description: "Updates the details of an existing customer identified by the provided ID.",
        operationId: nameof(UpdateCustomer),
        apiTag: [Constants.Tag],
        produces: [
            StatusCodes.Status200OK,
            StatusCodes.Status400BadRequest,
            StatusCodes.Status500InternalServerError
            ]);
    }

    internal abstract class UpdateCustomerValidator : AbstractValidator<Request>
    {
        protected UpdateCustomerValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Customer Id is required.");
            RuleFor(x => x.Name).NotEmpty().WithMessage("Customer name is required.");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("A valid email is required.");
        }
    }
}