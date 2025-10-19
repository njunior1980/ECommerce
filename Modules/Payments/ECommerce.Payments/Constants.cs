namespace ECommerce.Payments;

public static class Constants
{
    public const string DatabaseName = "DB_PAYMENTS";
    public const string Tag = "Payments";

    public static class ApiRoute
    {
        private const string Base = "api/payments";

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