using ECommerce.Payments.Domain;
using ECommerce.Shared.Core.Base;
using ECommerce.Shared.Infrastructure.CQRS;
using ECommerce.Shared.Infrastructure.RavenDB;
using Raven.Client.Documents;

namespace ECommerce.Payments.Queries;

public record PaymentResult(
    string Id, 
    string CreatedAt, 
    string OrderId, 
    decimal Amount, 
    string Status);

public record ListPaymentsQuery(int PageNumber = 1, int PageSize = 10) : IQuery<Result<IReadOnlyList<PaymentResult>>>;

public record ListPaymentsQueryHandler(IRavenDocumentStoreHolder StoreHolder) : IQueryHandler<ListPaymentsQuery, Result<IReadOnlyList<PaymentResult>>>
{
    public async Task<Result<IReadOnlyList<PaymentResult>>> Handle(ListPaymentsQuery command, CancellationToken ct = default)
    {
        using var session = StoreHolder.OpenSession(Constants.DatabaseName);
        {
            try
            {
                var payments = await session.Query<Payment>()
                    .Skip((command.PageNumber - 1) * command.PageSize)
                    .Take(command.PageSize)
                    .ToListAsync(ct);

                var result = payments.Select(p => new PaymentResult(
                    p.Id,
                    p.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                    p.OrderId,
                    p.Amount,
                    p.Status.ToString())).ToList();

                return Result.Success<IReadOnlyList<PaymentResult>>(result);
            }
            catch (Exception e)
            {
                return Result.Failure<IReadOnlyList<PaymentResult>>(Error.Exception(e.Message));
            }
        }
    }
}