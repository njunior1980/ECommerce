namespace ECommerce.Customers;

public static class Constants
{
    public const string DatabaseName = "DB_CUSTOMERS";
    public const string Tag = "Customers";

    public static class ApiRoute
    {
        private const string Base = "api/customers";
        public const string Create = Base;
        public const string Get = $"{Base}/{{id}}";        
    }
}