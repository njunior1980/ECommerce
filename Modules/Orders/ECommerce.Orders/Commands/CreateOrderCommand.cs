using ECommerce.Shared.Core.Base;
using ECommerce.Shared.Infrastructure.CQRS;
using ECommerce.Shared.Infrastructure.RavenDB;

namespace ECommerce.Orders.Commands;

public record CreateOrderCommand(
    string CustomerId, 
    string CustomerName, 
    string CustomerEmail, 
    Domain.ValueObjects.Address ShippingAddress, 
    IList<Domain.OrderItem> Items) : ICommand<Result<string>>;

public record CreateOrderCommandHandler(IRavenDocumentStoreHolder StoreHolder) : ICommandHandler<CreateOrderCommand, Result<string>>
{
    public async Task<Result<string>> Handle(CreateOrderCommand command, CancellationToken ct = default)
    {
        try
        {
            using(var session = StoreHolder.OpenSession(Constants.DatabaseName))
            {
                var order = new Domain.Order
                {
                    CustomerId = command.CustomerId,
                    CustomerName = command.CustomerName,
                    CustomerEmail = command.CustomerEmail,
                    ShippingAddress = command.ShippingAddress
                };

                foreach (var item in command.Items)
                    order.AddItem(item);

                await session.StoreAsync(order, order.Id, ct);
                await session.SaveChangesAsync(ct);

                return Result.Success(order.OrderNumber);
            }
        }
        catch (Exception e)
        {
           return Result.Failure<string>(Error.Exception(e.Message));
        }
    }
}