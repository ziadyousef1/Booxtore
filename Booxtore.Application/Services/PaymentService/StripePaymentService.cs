using Booxtore.Application.Interfaces.Services;
using Stripe;
using Stripe.Checkout;

namespace Booxtore.Application.Services.PaymentService
{
    public class StripePaymentService : IPaymentService
    {
    public StripePaymentService()
    {
    }        public async Task<string> CreatePaymentIntentAsync(decimal amount, string currency = "usd")
        {
            var options = new PaymentIntentCreateOptions
            {
                Amount = (long)(amount * 100),
                Currency = currency,
                AutomaticPaymentMethods = new PaymentIntentAutomaticPaymentMethodsOptions
                {
                    Enabled = true,
                },
            };

            var service = new PaymentIntentService();
            var paymentIntent = await service.CreateAsync(options);
            return paymentIntent.ClientSecret;
        }

        public async Task<bool> ConfirmPaymentAsync(string paymentIntentId)
        {
            try
            {
                var service = new PaymentIntentService();
                var paymentIntent = await service.GetAsync(paymentIntentId);
                return paymentIntent.Status == "succeeded";
            }
            catch
            {
                return false;
            }
        }

        public async Task<string> CreateCheckoutSessionAsync(int orderId, decimal amount, string successUrl, string cancelUrl)
        {
            try
            {
                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    LineItems = new List<SessionLineItemOptions>
                    {
                        new SessionLineItemOptions
                        {
                            PriceData = new SessionLineItemPriceDataOptions
                            {
                                UnitAmount = (long)(amount * 100),
                                Currency = "usd",
                                ProductData = new SessionLineItemPriceDataProductDataOptions
                                {
                                    Name = $"Booxtore Order #{orderId}",
                                    Description = "Digital Book Purchase",
                                },
                            },
                            Quantity = 1,
                        },
                    },
                    Mode = "payment",
                    SuccessUrl = successUrl,
                    CancelUrl = cancelUrl,
                    Metadata = new Dictionary<string, string>
                    {
                        { "order_id", orderId.ToString() }
                    }
                };

                var service = new SessionService();
                var session = await service.CreateAsync(options);
                return session.Url;
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create Stripe checkout session: {ex.Message}", ex);
            }
        }
    }
}
