using ECommerce.Customers.Domain;
using ECommerce.Shared.Core.Base;
using ECommerce.Shared.Infrastructure.CQRS;
using ECommerce.Shared.Infrastructure.RavenDB;

namespace ECommerce.Customers.Queries;

internal record GetCustomerResult(string Id, string Name);

internal record GetCustomerQuery(string Id) : IQuery<Result<GetCustomerResult>>;

internal class GetCustomerHandler(IRavenDocumentStoreHolder storeHolder) : IQueryHandler<GetCustomerQuery, Result<GetCustomerResult>>
{
    public async Task<Result<GetCustomerResult>> Handle(GetCustomerQuery command, CancellationToken ct = default)
    {
        try
        {
            var session = storeHolder.OpenSession(Constants.DatabaseName);

            var customer = await session.LoadAsync<Customer>(command.Id, ct);

            return customer is null
                ? Result.Failure<GetCustomerResult>(Error.NotFound("GetCustomer", $"Customer with id '{command.Id}' was not found."))
                : Result.Success(new GetCustomerResult(customer.Id, customer.Name));
        }
        catch (Exception e)
        {
           return Result.Failure<GetCustomerResult>(Error.Exception("Exception", e.Message));
        }
    }
}