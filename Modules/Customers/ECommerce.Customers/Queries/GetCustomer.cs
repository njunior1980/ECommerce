namespace ECommerce.Customers.Queries;

internal record GetCustomer(string Code);

internal record Result(string Code);

internal class GetCustomerHandler
{
    internal Task<Result> Handle(GetCustomer query)
    {
        // Implementation goes here
        return Task.FromResult(new Result(query.Code));
    }
}