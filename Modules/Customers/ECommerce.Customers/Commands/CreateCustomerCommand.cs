using ECommerce.Customers.Domain;
using ECommerce.Shared.Core.Base;
using ECommerce.Shared.Infrastructure.CQRS;
using ECommerce.Shared.Infrastructure.RavenDB;
using FluentValidation;

namespace ECommerce.Customers.Commands;

internal record CreateCustomerCommand(string Name, string Email) : IQuery<Result<string>>;

internal class AddCustomerValidation : AbstractValidator<CreateCustomerCommand>
{
    public AddCustomerValidation()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
        RuleFor(x => x.Email).NotEmpty().EmailAddress().WithMessage("A valid email is required.");
    }
}

internal class AddCustomerHandler(IRavenDocumentStoreHolder storeHolder) : IQueryHandler<CreateCustomerCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateCustomerCommand command, CancellationToken ct = default)
    {
        try
        {
            var session = storeHolder.OpenSession(Constants.DatabaseName);

            var customer = new Customer();
            customer.Create(command.Name, command.Email);

            await session.StoreAsync(customer, customer.Id, ct);
            await session.SaveChangesAsync(ct);

            return Result.Success(customer.Id);
        }
        catch (Exception e)
        {
            return Result.Failure<string>(Error.Exception("Exception", e.Message));
        }
    }
}