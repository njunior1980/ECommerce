using ECommerce.Shared.Core.Base;
using ECommerce.Shared.Infrastructure.CQRS;
using ECommerce.Shared.Infrastructure.RavenDB;
using ECommerce.Shared.Infrastructure.Stripe;
using Stripe;

namespace ECommerce.Payments.Commands;

public record CreatePaymentCommand(
    string OrderId,
    string CustomerId,
    string CustomerName,
    string CustomerEmail,
    string CustomerPhone,    
    decimal Amount) : ICommand<Result<bool>>;

internal class CreatePaymentHandler(IRavenDocumentStoreHolder storeHolder, IStripeService stripeService): ICommandHandler<CreatePaymentCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            using(var session = storeHolder.OpenSession(Constants.DatabaseName))
            {
                var payment = new Domain.Payment
                {
                    OrderId = request.OrderId,
                    CustomerId = request.CustomerId,
                    CustomerName = request.CustomerName,
                    CustomerEmail = request.CustomerEmail,
                    CustomerPhone = request.CustomerPhone,
                    Amount = request.Amount                    
                };

                await session.StoreAsync(payment, payment.Id, cancellationToken);
                await session.SaveChangesAsync(cancellationToken);
                
                return Result.Success(true);
            }
        }
        catch (Exception e)
        {
            return Result.Failure<bool>(Error.Exception(e.Message));
        }
    }

    private async Task<bool> ProcessingPayment(IStripeService stripeService)
    {
        try
        {
            //var paymentIntent = await stripeService .CreatePaymentIntent(new PaymentIntentCreateOptions
            //{
            //    Amount = (long)(payment.Amount * 100), // Convert to cents
            //    Currency = "usd",
            //    PaymentMethodTypes = new List<string> { "card" }
            //});

            //return paymentIntent.Status == "succeeded";
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }
}