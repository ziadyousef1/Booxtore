using Microsoft.AspNetCore.Identity;
using Booxtore.Domain.Models;
using Booxtore.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Booxtore.Infrastructure.Data
{
    public static class BooxtoreSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<BooxtoreContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (!await roleManager.RoleExistsAsync("User"))
            {
                await roleManager.CreateAsync(new IdentityRole("User"));
            }

            var adminEmail = "admin@booxtore.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Admin",
                    LastName = "User",
                    EmailConfirmed = true,
                    Status = "Active",
                    CreatedAt = DateTime.UtcNow
                };

                await userManager.CreateAsync(adminUser, "Admin123!");
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            var userEmail = "user@booxtore.com";
            var normalUser = await userManager.FindByEmailAsync(userEmail);
            if (normalUser == null)
            {
                normalUser = new ApplicationUser
                {
                    UserName = userEmail,
                    Email = userEmail,
                    FirstName = "Jane",
                    LastName = "Reader",
                    EmailConfirmed = true,
                    Status = "Active",
                    CreatedAt = DateTime.UtcNow
                };

                await userManager.CreateAsync(normalUser, "User123!");
                await userManager.AddToRoleAsync(normalUser, "User");
            }

            await context.SaveChangesAsync();
        }
    }
}
