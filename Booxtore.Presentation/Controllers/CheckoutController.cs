using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Booxtore.Application.Interfaces.Services;
using Booxtore.Domain.Models;
using Booxtore.Presentation.ViewModels;

namespace Booxtore.Presentation.Controllers
{
    [Authorize]
    public class CheckoutController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;
        private readonly IPaymentService _paymentService;
        private readonly UserManager<ApplicationUser> _userManager;

        public CheckoutController(
            ICartService cartService,
            IOrderService orderService,
            IPaymentService paymentService,
            UserManager<ApplicationUser> userManager)
        {
            _cartService = cartService;
            _orderService = orderService;
            _paymentService = paymentService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var cart = await _cartService.GetCartAsync();
            
            if (cart.TotalItems == 0)
            {
                TempData["Error"] = "Your cart is empty";
                return RedirectToAction("Index", "Cart");
            }

            var user = await _userManager.GetUserAsync(User);
            
            var viewModel = new CheckoutViewModel
            {
                Cart = cart,
                User = user,
                TotalAmount = cart.TotalAmount
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrder()
        {
            try
            {
                var cart = await _cartService.GetCartAsync();
                
                if (cart.TotalItems == 0)
                {
                    TempData["Error"] = "Your cart is empty";
                    return RedirectToAction("Index", "Cart");
                }

                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    TempData["Error"] = "User not found. Please log in again.";
                    return RedirectToAction("Login", "Auth");
                }

                var order = await _orderService.CreateOrderAsync(user.Id, cart);

                var successUrl = Url.Action("Success", "Checkout", new { orderId = order.OrderId }, Request.Scheme);
                var cancelUrl = Url.Action("Cancel", "Checkout", new { orderId = order.OrderId }, Request.Scheme);
                
                try
                {
                    var checkoutUrl = await _paymentService.CreateCheckoutSessionAsync(
                        order.OrderId, 
                        order.TotalAmount.Value, 
                        successUrl, 
                        cancelUrl);

                    if (string.IsNullOrEmpty(checkoutUrl))
                    {
                        throw new Exception("Failed to create Stripe checkout session");
                    }

                    return Redirect(checkoutUrl);
                }
                catch (Exception stripeEx)
                {
                    await _orderService.UpdateOrderStatusAsync(order.OrderId, "Failed");
                    TempData["Error"] = "Payment processing is temporarily unavailable. Please try again later.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error creating order: {ex.Message}";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Success(int orderId)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(orderId);
                if (order == null)
                {
                    return NotFound();
                }

                await _orderService.UpdateOrderStatusAsync(orderId, "Completed", "Stripe");
                
                await _cartService.ClearCartAsync();

                var viewModel = new OrderSuccessViewModel
                {
                    Order = order
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error processing order: {ex.Message}";
                return RedirectToAction("Index", "Cart");
            }

        }

        public async Task<IActionResult> Cancel(int orderId)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(orderId);
                if (order != null)
                {
                    await _orderService.UpdateOrderStatusAsync(orderId, "Cancelled");
                }

                TempData["Warning"] = "Payment was cancelled. Your order has been cancelled.";
                return RedirectToAction("Index", "Cart");
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error cancelling order: {ex.Message}";
                return RedirectToAction("Index", "Cart");
            }
        }
    }
}
