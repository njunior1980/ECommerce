namespace ECommerce.Orders;

public static class Constants
{
    public const string DatabaseName = "DB_ORDERS";
    public const string Tag = "Orders";

    public static class ApiRoute
    {
        private const string Base = "api/orders";
        public const string Get = $"{Base}/{{id}}";
        public const string List = $"{Base}/{{customerId}}";
        public const string Create = Base;
    }

    public static class Messages
    {
        public const string OrderCreated = "Order created successfully.";
    }

    public static class Errors
    {
        public const string OrderNotFound = "Order Number with '{0}' was not found.";
        public const string RequestCancelled = "Request was cancelled.";
    }
}