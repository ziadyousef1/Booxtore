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

            // Seed Categories
            if (!context.Categories.Any())
            {
                var categories = new List<Category>
                {
                    new Category
                    {
                        Name = "Fiction",
                        Description = "Fictional stories and novels",
                        CreatedAt = DateTime.UtcNow
                    },
                    new Category
                    {
                        Name = "Non-Fiction",
                        Description = "Factual books and educational content",
                        CreatedAt = DateTime.UtcNow
                    },
                    new Category
                    {
                        Name = "Science Fiction",
                        Description = "Science fiction and fantasy novels",
                        CreatedAt = DateTime.UtcNow
                    },
                    new Category
                    {
                        Name = "Biography",
                        Description = "Life stories and memoirs",
                        CreatedAt = DateTime.UtcNow
                    },
                    new Category
                    {
                        Name = "Technology",
                        Description = "Books about technology and programming",
                        CreatedAt = DateTime.UtcNow
                    },
                    new Category
                    {
                        Name = "Business",
                        Description = "Business and entrepreneurship books",
                        CreatedAt = DateTime.UtcNow
                    },
                    new Category
                    {
                        Name = "History",
                        Description = "Historical books and documentaries",
                        CreatedAt = DateTime.UtcNow
                    },
                    new Category
                    {
                        Name = "Romance",
                        Description = "Romantic novels and stories",
                        CreatedAt = DateTime.UtcNow
                    }
                };

                context.Categories.AddRange(categories);
                await context.SaveChangesAsync();
            }

            // Seed Authors
            if (!context.Authors.Any())
            {
                var authors = new List<Author>
                {
                    new Author
                    {
                        Name = "J.K. Rowling",
                        Biography = "British author best known for the Harry Potter series.",
                        BirthDate = new DateOnly(1965, 7, 31),
                        CreatedAt = DateTime.UtcNow
                    },
                    new Author
                    {
                        Name = "Stephen King",
                        Biography = "American author of horror, supernatural fiction, suspense, crime, science-fiction, and fantasy novels.",
                        BirthDate = new DateOnly(1947, 9, 21),
                        CreatedAt = DateTime.UtcNow
                    },
                    new Author
                    {
                        Name = "Agatha Christie",
                        Biography = "English writer known for her detective novels, particularly those featuring Hercule Poirot and Miss Marple.",
                        BirthDate = new DateOnly(1890, 9, 15),
                        CreatedAt = DateTime.UtcNow
                    },
                    new Author
                    {
                        Name = "George Orwell",
                        Biography = "English novelist and essayist, journalist and critic, best known for his novels Animal Farm and Nineteen Eighty-Four.",
                        BirthDate = new DateOnly(1903, 6, 25),
                        CreatedAt = DateTime.UtcNow
                    },
                    new Author
                    {
                        Name = "Jane Austen",
                        Biography = "English novelist known primarily for her six major novels which interpret and critique the British landed gentry.",
                        BirthDate = new DateOnly(1775, 12, 16),
                        CreatedAt = DateTime.UtcNow
                    },
                    new Author
                    {
                        Name = "Mark Twain",
                        Biography = "American writer, humorist, entrepreneur, publisher, and lecturer.",
                        BirthDate = new DateOnly(1835, 11, 30),
                        CreatedAt = DateTime.UtcNow
                    },
                    new Author
                    {
                        Name = "Ernest Hemingway",
                        Biography = "American novelist, short-story writer, and journalist.",
                        BirthDate = new DateOnly(1899, 7, 21),
                        CreatedAt = DateTime.UtcNow
                    },
                    new Author
                    {
                        Name = "Toni Morrison",
                        Biography = "American novelist, essayist, book editor, and college professor.",
                        BirthDate = new DateOnly(1931, 2, 18),
                        CreatedAt = DateTime.UtcNow
                    },
                    new Author
                    {
                        Name = "Harper Lee",
                        Biography = "American novelist widely known for To Kill a Mockingbird.",
                        BirthDate = new DateOnly(1926, 4, 28),
                        CreatedAt = DateTime.UtcNow
                    },
                    new Author
                    {
                        Name = "F. Scott Fitzgerald",
                        Biography = "American novelist and short story writer, whose works are the paradigmatic writings of the Jazz Age.",
                        BirthDate = new DateOnly(1896, 9, 24),
                        CreatedAt = DateTime.UtcNow
                    }
                };

                context.Authors.AddRange(authors);
                await context.SaveChangesAsync();
            }

            await context.SaveChangesAsync();
        }
    }
}
