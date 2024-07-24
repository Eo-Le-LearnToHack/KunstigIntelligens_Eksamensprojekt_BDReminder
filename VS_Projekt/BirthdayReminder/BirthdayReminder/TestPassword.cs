using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace BirthdayReminder
{
    public static class TestPassword
    {
        public static async Task CheckAdminPassword(IServiceProvider services)
        {
            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
            var logger = services.GetRequiredService<ILogger<Program>>();

            var adminEmail = "admin@example.com";
            var password = "Admin123!";

            var user = await userManager.FindByEmailAsync(adminEmail);
            if (user != null)
            {
                var result = await userManager.CheckPasswordAsync(user, password);
                if (result)
                {
                    logger.LogInformation("Password is correct.");
                }
                else
                {
                    logger.LogError("Password is incorrect.");
                }
            }
            else
            {
                logger.LogError("User not found.");
            }
        }
    }
}
