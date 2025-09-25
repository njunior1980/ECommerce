using Microsoft.Extensions.Logging;
using Stripe;

namespace ECommerce.Shared.Infrastructure.Stripe;

public class StripeService(ILogger<StripeService> logger, IStripeClient stripeClient) : IStripeService
{
    public async Task<Customer> CreateCustomerAsync(string customerName, string customerEmail, CancellationToken ct = default)
    {
        try
        {
            ct.ThrowIfCancellationRequested();

            var options = new CustomerCreateOptions
            {
                Name = customerName.Trim(),
                Email = customerEmail.Trim()
            };

            var service = new CustomerService(stripeClient);
            return await service.CreateAsync(options, cancellationToken: ct);
        }
        catch (Exception e)
        {
            logger.LogError("CreateCustomerAsync Error: {ex}", e.Message);
            throw;
        }
    }
    public async Task<bool> CheckIfCardExistsAsync(string stripeCustomer, string last4Digits, string expMonth, string expYear, CancellationToken ct = default)
    {
        try
        {
            ct.ThrowIfCancellationRequested();

            var paymentMethodListOptions = new PaymentMethodListOptions
            {
                Customer = stripeCustomer,
                Type = "card"
            };

            var service = new PaymentMethodService(stripeClient);
            var result = await service.ListAsync(paymentMethodListOptions, cancellationToken: ct);

            var exists = result?.Data.Any(p => p.Card.Last4 == last4Digits &&
                                               p.Card.ExpMonth == long.Parse(expMonth) &&
                                               p.Card.ExpYear == long.Parse(expYear));

            return Convert.ToBoolean(exists);
        }
        catch (Exception e)
        {
            logger.LogError("CheckIfExistsAsync Error: {ex}", e.Message);
            throw;
        }
    }

    public async Task<Card> AddCardAsync(string customerId, string cardNumber, string expMonth, 
        string expYear, string cvc, string cardBrand, CancellationToken ct = default)
    {
        try
        {
            ct.ThrowIfCancellationRequested();

            var tokenOptions = new TokenCreateOptions
            {
                Card = new TokenCardOptions
                {
                    Name = cardBrand.Trim(),
                    Number = cardNumber.Trim(),
                    ExpMonth = expMonth.Trim(),
                    ExpYear = expYear.Trim(),
                    Cvc = cvc.Trim()
                }
            };

            var tokenService = new TokenService(stripeClient);
            var stripeToken = await tokenService.CreateAsync(tokenOptions, cancellationToken: ct);

            var cardOptions = new CardCreateOptions
            {
                Source = stripeToken.Id,
            };

            var cardService = new CardService(stripeClient);
            return await cardService.CreateAsync(customerId, cardOptions, cancellationToken: ct);
        }
        catch (Exception e)
        {
            logger.LogError("AddCardAsync Error: {ex}", e.Message);
            throw;
        }
    }

    public async Task DeleteCardAsync(string customerId, string cardId, CancellationToken ct = default)
    {
        try
        {
            ct.ThrowIfCancellationRequested();
            var service = new CardService(stripeClient);
            await service.DeleteAsync(customerId, cardId, cancellationToken: ct);
        }
        catch (Exception e)
        {
            logger.LogError("DeleteCardAsync Error: {ex}", e.Message);
            throw;
        }
    }
}