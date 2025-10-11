using ECommerce.Customers.Commands;
using ECommerce.Shared.Core.Endpoints;
using ECommerce.Shared.Infrastructure.CQRS;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace ECommerce.Customers.Endpoints;

public class CreateCustomer : IEndpoint
{
    internal record Request(string Name, string Email);

    public void MapEndpoint(IEndpointRouteBuilder builder)
    {
        builder.MapPost(Constants.ApiRoute.Create, async (Request request, IDispatcher dispatcher, CancellationToken ct) =>
        {
            try
            {
                var command = new CreateCustomerCommand(request.Name, request.Email);

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
            summary: "Create a new customer",
            description: "Creates a new customer with the provided name and email.",
            operationId: nameof(CreateCustomer),
            apiTag: [Constants.Tag],
            produces: [
                StatusCodes.Status200OK,
                StatusCodes.Status400BadRequest,
                StatusCodes.Status500InternalServerError
                ]);
    }

    internal class CreateCustomerValidation : AbstractValidator<Request>
    {
        public CreateCustomerValidation()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("A valid email is required.");
        }
    }
}