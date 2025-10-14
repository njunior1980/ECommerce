namespace ECommerce.Catalog;

public static class Constants
{
    public const string DatabaseName = "DB_CATALOG";
    public const string Tag = "Catalog";

    public static class ApiRoute
    {
        private const string Base = "api/catalog";

        public const string GetCategory = $"{Base}/categories/{{id}}";
        public const string ListCategory = $"{Base}/categories";
        public const string CreateCategory = $"{Base}/categories";
        public const string UpdateCategory = $"{Base}/{{id}}";

        public const string GetProduct = $"{Base}/products/{{id}}";
        public const string ListProduct = $"{Base}/products";
        public const string CreateProduct = $"{Base}/products";
        public const string UpdateProduct = $"{Base}/products/{{id}}";
    }

    public static class Messages
    {

    }

    public static class Errors
    {
        public const string CustomerNotFound = "Customer with ID '{0}' was not found.";
    }
}