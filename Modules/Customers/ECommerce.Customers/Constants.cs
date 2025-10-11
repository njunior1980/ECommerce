namespace ECommerce.Customers;

public static class Constants
{
    public const string DatabaseName = "DB_CUSTOMERS";
    public const string Tag = "Customers";

    public static class ApiRoute
    {
        private const string Base = "api/customers";
        public const string Get = $"{Base}/{{id}}";
        public const string Create = Base;
        public const string Update = $"{Base}/{{id}}";
    }

    public static class Messages
    {

    }

    public static class Errors
    {
        public const string CustomerNotFound = "Customer with ID '{0}' was not found.";
    }
}