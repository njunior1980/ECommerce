using ECommerce.Shared.Core.Base;
using ECommerce.Shared.Infrastructure.CQRS;
using ECommerce.Shared.Infrastructure.RavenDB;

namespace ECommerce.Customers.Commands;

public record RemoveAccountCommand(string Id) : ICommand<Result>;

public class RemoveAccountCommandHandler(IRavenDocumentStoreHolder storeHolder) : ICommandHandler<RemoveAccountCommand, Result>
{
    public async Task<Result> Handle(RemoveAccountCommand request, CancellationToken cancellationToken)
    {
        using (var session = storeHolder.OpenSession(Constants.DatabaseName))
        {
            try
            {
                var customer = await session.LoadAsync<Domain.Customer>(request.Id, cancellationToken);

                if (customer is null)
                {
                    return Result.Failure(Error.NotFound("Customer.NotFound", string.Format(Constants.Errors.CustomerNotFound, request.Id)));
                }

                session.Delete(customer);
                await session.SaveChangesAsync(cancellationToken);
                return Result.Success();
            }
            catch (Exception e)
            {
                return Result.Failure<Result>(Error.Exception("Exception", e.Message));
            }
        }
    }
}