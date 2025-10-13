using ECommerce.Orders.Domain;
using ECommerce.Shared.Core.Base;
using ECommerce.Shared.Infrastructure.CQRS;
using ECommerce.Shared.Infrastructure.RavenDB;
using Raven.Client.Documents;

namespace ECommerce.Orders.Queries;

public record ListOrdersQuery(
    string CustomerId, 
    DateTime? CreatedAtFrom = null, 
    DateTime? CreatedAtTo = null, 
    OrderStatus? Status = null) : IQuery<Result<IList<Order>>>;

public record ListOrdersQueryHandler(IRavenDocumentStoreHolder StoreHolder) : IQueryHandler<ListOrdersQuery, Result<IList<Order>>>
{
    public async Task<Result<IList<Order>>> Handle(ListOrdersQuery command, CancellationToken ct = default)
    {
        using(var session = StoreHolder.OpenSession(Constants.DatabaseName))
        {
            try
            {
                IQueryable<Order> query = session.Query<Order>();

                query = query.Where(o => o.CustomerId == command.CustomerId);

                if (command.CreatedAtFrom.HasValue)
                {
                    query = query.Where(o => o.CreatedAt >= command.CreatedAtFrom.Value);
                }

                if (command.CreatedAtTo.HasValue)
                {
                    query = query.Where(o => o.CreatedAt <= command.CreatedAtTo.Value);
                }

                if (command.Status.HasValue)
                {
                    query = query.Where(o => o.Status == command.Status.Value);
                }

                var orders = await query.ToListAsync(ct);

                return Result.Success<IList<Order>>(orders);
            }
            catch (Exception e)
            {
                return Result.Failure<IList<Order>>(Error.Exception(e.Message));
            }
        }
    }
}