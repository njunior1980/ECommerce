using Stripe;

namespace ECommerce.Shared.Infrastructure.Stripe;

public interface IStripeService
{
    Task<Customer> CreateCustomerAsync(string customerName, string customerEmail, CancellationToken ct = default);
    Task<bool> CheckIfCardExistsAsync(string stripeCustomer, string last4Digits, string expMonth, string expYear, CancellationToken ct = default);
    Task<Card> AddCardAsync(string customerId, string cardNumber, string expMonth, string expYear, string cvc, string cardBrand, CancellationToken ct = default);
    Task DeleteCardAsync(string customerId, string cardId, CancellationToken ct = default);
}