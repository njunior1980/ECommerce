using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace ECommerce.Shared.Core.Endpoints.Filters;

public class RequestValidationFilter<TRequest>(ILogger<RequestValidationFilter<TRequest>> logger, IValidator<TRequest> validator) : IEndpointFilter
{
    public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var requestName = typeof(TRequest).FullName;

        if (validator is null)
        {
            var error = $"{requestName}: No validator configured.";

            logger.LogError(error);

            return TypedResults.BadRequest(error);
        }

        var request = context.Arguments.OfType<TRequest>().First();

        var validationResult = await validator.ValidateAsync(request, context.HttpContext.RequestAborted);

        if (validationResult.IsValid)
            return await next(context);

        logger.LogError("{Request}: Validation failed.", requestName);

        return TypedResults.ValidationProblem(validationResult.ToDictionary());
    }
}