using ECommerce.Payments.Domain.ValueObjects;
using ECommerce.Shared.Core.Base;
using ECommerce.Shared.Infrastructure.CQRS;
using ECommerce.Shared.Infrastructure.Stripe;

namespace ECommerce.Payments.Commands;

public record AddCreditCardCommand(
    string CustomerId,
    string CardNumber,
    string CardHolderName,
    string ExpirationDate,
    string Cvc,
    CreditCardBrand Brand) : ICommand<Result<bool>>;

internal class AddCreditCardHandler(IStripeService stripeService) : ICommandHandler<AddCreditCardCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(AddCreditCardCommand command, CancellationToken ct = default)
    {
        try
        {
            var last4Digits = command.CardNumber[^4..];
            var expMonth = command.ExpirationDate.Split('/')[0];
            var expYear = command.ExpirationDate.Split('/')[1];

            var hasCreditCard = await stripeService.CheckIfCardExistsAsync(command.CustomerId, last4Digits, expMonth, expYear, ct);

            if (hasCreditCard)
            {
                return Result.Failure<bool>(Error.Failure("Payments.AddCreditCard", "The credit card already exists for this customer."));
            }

            var card = await stripeService.AddCardAsync(
                command.CustomerId,
                command.CardNumber,
                expMonth,
                expYear,
                command.Cvc,
                command.Brand.ToString(),
                ct);

            return card.Id is null
                ? Result.Failure<bool>(Error.Failure("Payments.AddCreditCard", "Failed to add the credit card."))
                : Result.Success(true);
        }
        catch (Exception e)
        {
            return Result.Failure<bool>(Error.Exception(e.Message));
        }
    }
}