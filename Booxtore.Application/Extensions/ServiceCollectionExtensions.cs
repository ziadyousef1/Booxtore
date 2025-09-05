using Booxtore.Application.Interfaces.Services;
using BookServiceImplementation = Booxtore.Application.Services.BookService;
using CategoryServiceImplementation = Booxtore.Application.Services.CategoryService;
using UserServiceImplementation = Booxtore.Application.Services.UserService;
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
            
       

            return services;
        }
    }
}
