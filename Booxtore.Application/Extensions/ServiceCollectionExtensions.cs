using Booxtore.Application.Interfaces.Services;
using BookServiceImplementation = Booxtore.Application.Services.BookService;
using CategoryServiceImplementation = Booxtore.Application.Services.CategoryService;
using UserServiceImplementation = Booxtore.Application.Services.UserService;
using CartServiceImplementation = Booxtore.Application.Services.CartService;
using OrderServiceImplementation = Booxtore.Application.Services.OrderService;
using PaymentServiceImplementation = Booxtore.Application.Services.PaymentService;
using Microsoft.Extensions.DependencyInjection;

namespace Booxtore.Application.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IBookService, BookServiceImplementation.BookService>();
            services.AddScoped<ICategoryService, CategoryServiceImplementation.CategoryService>();
            services.AddScoped<IUserService, UserServiceImplementation.UserService>();
            services.AddScoped<ICartService, CartServiceImplementation.CartService>();
            services.AddScoped<IOrderService, OrderServiceImplementation.OrderService>();
            services.AddScoped<IPaymentService, PaymentServiceImplementation.StripePaymentService>();

            return services;
        }
    }
}
