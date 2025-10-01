using ECommerce.Shared.Core.Base;
using ECommerce.Shared.Infrastructure.CQRS;

namespace ECommerce.Customers.Commands;

internal record AddCustomerCommand(string Name, string Email) : IQuery<Result<string>>;

internal class AddCustomerValidation
{

}
internal class AddCustomerHandler : IQueryHandler<AddCustomerCommand, Result<string>>
{
    public Task<Result<string>> Handle(AddCustomerCommand query, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}