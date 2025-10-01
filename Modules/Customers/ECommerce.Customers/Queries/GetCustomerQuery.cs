using ECommerce.Shared.Core.Base;
using ECommerce.Shared.Infrastructure.CQRS;

namespace ECommerce.Customers.Queries;

internal record GetCustomerResult(string Id, string Name);

internal record GetCustomerQuery(string Id) : IQuery<Result<GetCustomerResult>>;

internal class GetCustomerHandler : IQueryHandler<GetCustomerQuery, Result<GetCustomerResult>>
{
    public Task<Result<GetCustomerResult>> Handle(GetCustomerQuery query, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}