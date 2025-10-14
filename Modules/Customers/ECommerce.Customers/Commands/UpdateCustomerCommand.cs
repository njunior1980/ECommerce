using ECommerce.Customers.Domain;
using ECommerce.Shared.Core.Base;
using ECommerce.Shared.Infrastructure.CQRS;
using ECommerce.Shared.Infrastructure.RavenDB;

namespace ECommerce.Customers.Commands;

internal record UpdateCustomerCommand(string Id, string Name, string Email, string Phone) : ICommand<Result<string>>;

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

            customer.Update(command.Name, command.Email, command.Phone);

            await session.SaveChangesAsync(ct);

            return Result.Success(customer.Id);

        }
        catch (Exception e)
        {
            return Result.Failure<string>(Error.Exception("Exception", e.Message));
        }
    }
}