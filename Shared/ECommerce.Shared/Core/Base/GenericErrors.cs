namespace ECommerce.Shared.Core.Base;

public static class GenericErrors
{
    public static Error Failure(Exception exception) =>
        Error.Exception("Application Exception", $"Occurred an error: {exception.Message}.");

    public static Error NotFound(string value) 
        => Error.NotFound($"{value}.NotFound", $"{value} not found!");

    public static Error UnprocessableEntity(string value, string message = null)
        => Error.Failure($"{value}.UnprocessableEntity", !string.IsNullOrWhiteSpace(message)
            ? message
            : "The request was valid, but the data is incorrect.");
}