using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stripe;

namespace ECommerce.Shared.Infrastructure.Stripe;

public static class StripeExtensions
{
    public static void AddStripeService(this IServiceCollection services)
    {
        var configuration = services.BuildServiceProvider().GetService<IConfiguration>();
        var apiKey = configuration?.GetSection("StripeApiKey").Value;

        services.AddSingleton<IStripeClient>(new StripeClient(apiKey));
        services.AddSingleton<IStripeService, StripeService>();
    }
}