using ECommerce.Customers.Domain;
using ECommerce.Shared.Core.Base;
using ECommerce.Shared.Infrastructure.CQRS;
using ECommerce.Shared.Infrastructure.RavenDB;
using FluentValidation;

namespace ECommerce.Customers.Commands;

internal record UpdateCustomerCommand(string Id, string Name, string Email) : ICommand<Result<string>>;

internal abstract class UpdateCustomerValidator: AbstractValidator<UpdateCustomerCommand>
{
    protected UpdateCustomerValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Customer Id is required.");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Customer name is required.");
        RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("A valid email is required.");
    }
}

internal class UpdateCustomerHandler(IRavenDocumentStoreHolder storeHolder) : ICommandHandler<UpdateCustomerCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UpdateCustomerCommand command, CancellationToken ct = default)
    {
        try
        {
            var session = storeHolder.OpenSession(Constants.DatabaseName);

            var customer = await session.LoadAsync<Customer>(command.Id, ct);

            if (customer is null)
            {
                return Result.Failure<string>(Error.NotFound("UpdateCustomer", $"Customer with id '{command.Id}' was not found."));
            }

            customer.Update(command.Name, command.Email);

            await session.SaveChangesAsync(ct);

            return Result.Success(customer.Id);

        }
        catch (Exception e)
        {
            return Result.Failure<string>(Error.Exception("Exception", e.Message));
        }
    }
}