namespace ECommerce.Shared.Infrastructure.CQRS;

internal class Dispatcher(IServiceProvider serviceProvider) : IDispatcher
{
    public Task<TResult> Send<TResult>(ICommand<TResult> command, CancellationToken ct = default)
    {
        var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResult));
        var handler = serviceProvider.GetService(handlerType);

        if (handler is null)
        {
            throw new InvalidOperationException($"Handler for command {command.GetType().Name} not found");
        }

        var method = handlerType.GetMethod("Handle");
        return (Task<TResult>)method?.Invoke(handler, [command, ct])!;
    }

    public Task<TResult> Send<TResult>(IQuery<TResult> query, CancellationToken ct = default)
    {
        var handlerType = typeof(IQueryHandler<,>).MakeGenericType(query.GetType(), typeof(TResult));
        var handler = serviceProvider.GetService(handlerType);

        if (handler is null)
        {
            throw new InvalidOperationException($"Handler for query {query.GetType().Name} not found");
        }

        var method = handlerType.GetMethod("Handle");
        return (Task<TResult>)method?.Invoke(handler, [query, ct])!;
    }
}