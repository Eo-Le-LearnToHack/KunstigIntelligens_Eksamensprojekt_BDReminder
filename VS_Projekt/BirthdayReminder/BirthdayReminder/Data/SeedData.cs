using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using BirthdayReminder.Models;

namespace BirthdayReminder.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string adminEmail = "admin@example.com";
            string password = "Admin123!";

            // Seed roles
            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }

            // Seed admin user
            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                var admin = new IdentityUser { UserName = adminEmail, Email = adminEmail };
                var result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }

            // Seed birthdays
            if (!context.Birthdays.Any())
            {
                context.Birthdays.AddRange(
                    new Birthday
                    {
                        Name = "John Doe",
                        Date = new DateTime(1990, 1, 1),
                        Relationship = "Friend"
                    },
                    new Birthday
                    {
                        Name = "Jane Smith",
                        Date = new DateTime(1985, 5, 23),
                        Relationship = "Sister"
                    }
                );

                await context.SaveChangesAsync();
            }
        }
    }
}
