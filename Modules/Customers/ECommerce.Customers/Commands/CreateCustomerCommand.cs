using ECommerce.Customers.Domain;
using ECommerce.Shared.Core.Base;
using ECommerce.Shared.Infrastructure.CQRS;
using ECommerce.Shared.Infrastructure.RavenDB;

namespace ECommerce.Customers.Commands;

internal record CreateCustomerCommand(string Name, string Email, string Phone) : IQuery<Result<string>>;

internal class AddCustomerHandler(IRavenDocumentStoreHolder storeHolder) : IQueryHandler<CreateCustomerCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateCustomerCommand command, CancellationToken ct = default)
    {
        try
        {
            var session = storeHolder.OpenSession(Constants.DatabaseName);

            var customer = Customer.Create(command.Name, command.Email, command.Phone);

            await session.StoreAsync(customer, customer.Id, ct);
            await session.SaveChangesAsync(ct);

            return Result.Success(customer.Id);
        }
        catch (Exception e)
        {
            return Result.Failure<string>(Error.Exception(e.Message));
        }
    }
}