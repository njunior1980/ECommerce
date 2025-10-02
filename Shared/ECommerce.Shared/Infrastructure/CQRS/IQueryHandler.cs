namespace ECommerce.Shared.Infrastructure.CQRS;

public interface IQueryHandler<in TQuery, TResult> where TQuery : IQuery<TResult>
{
    Task<TResult> Handle(TQuery command, CancellationToken ct = default);
}