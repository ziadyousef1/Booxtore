namespace Booxtore.Application.Interfaces.Services
{
    public interface IPaymentService
    {
        Task<string> CreatePaymentIntentAsync(decimal amount, string currency = "usd");
        Task<bool> ConfirmPaymentAsync(string paymentIntentId);
        Task<string> CreateCheckoutSessionAsync(int orderId, decimal amount, string successUrl, string cancelUrl);
    }
}
