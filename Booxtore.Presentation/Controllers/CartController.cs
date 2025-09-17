using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Booxtore.Application.Interfaces.Services;
using Booxtore.Presentation.ViewModels;

namespace Booxtore.Presentation.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IBookService _bookService;

        public CartController(ICartService cartService, IBookService bookService)
        {
            _cartService = cartService;
            _bookService = bookService;
        }

        public async Task<IActionResult> Index()
        {
            var cart = await _cartService.GetCartAsync();
            var viewModel = new CartViewModel
            {
                Items = cart.Items,
                TotalAmount = cart.TotalAmount,
                TotalItems = cart.TotalItems
            };
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int bookId, int quantity = 1)
        {
            try
            {
                await _cartService.AddToCartAsync(bookId, quantity);
                
                var cartCount = await _cartService.GetCartItemCountAsync();
                
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, cartCount = cartCount, message = "Item added to cart successfully!" });
                }
                
                TempData["Success"] = "Item added to cart successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = ex.Message });
                }
                
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index", "Shop");
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateQuantity(int bookId, int quantity)
        {
            try
            {
                await _cartService.UpdateQuantityAsync(bookId, quantity);
                
                var cart = await _cartService.GetCartAsync();
                var item = cart.Items.FirstOrDefault(x => x.BookId == bookId);
                
                return Json(new { 
                    success = true, 
                    itemTotal = item?.Total ?? 0,
                    cartTotal = cart.TotalAmount,
                    cartCount = cart.TotalItems
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int bookId)
        {
            try
            {
                await _cartService.RemoveFromCartAsync(bookId);
                
                var cart = await _cartService.GetCartAsync();
                
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { 
                        success = true, 
                        cartTotal = cart.TotalAmount,
                        cartCount = cart.TotalItems,
                        message = "Item removed from cart" 
                    });
                }
                
                TempData["Success"] = "Item removed from cart";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = ex.Message });
                }
                
                TempData["Error"] = ex.Message;
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> ClearCart()
        {
            await _cartService.ClearCartAsync();
            TempData["Success"] = "Cart cleared successfully";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> IsBookInCart(int bookId)
        {
            try
            {
                var cart = await _cartService.GetCartAsync();
                var isInCart = cart.Items.Any(item => item.BookId == bookId);
                
                return Json(new { success = true, isInCart = isInCart });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, isInCart = false });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCartCount()
        {
            var count = await _cartService.GetCartItemCountAsync();
            return Json(new { count });
        }
    }
}
