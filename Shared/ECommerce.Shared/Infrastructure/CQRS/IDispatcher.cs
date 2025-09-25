namespace ECommerce.Shared.Infrastructure.CQRS;

public interface IDispatcher
{
    Task<TResult> Send<TResult>(ICommand<TResult> command, CancellationToken ct = default);
    Task<TResult> Send<TResult>(IQuery<TResult> query, CancellationToken ct = default);
}