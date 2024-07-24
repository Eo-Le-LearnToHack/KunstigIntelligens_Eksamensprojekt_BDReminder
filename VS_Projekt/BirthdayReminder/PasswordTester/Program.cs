using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
}

public class Program
{
    public static async Task Main(string[] args)
    {
        var services = new ServiceCollection();
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=BirthdayReminder;Trusted_Connection=True;MultipleActiveResultSets=true"));
        services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();
        services.AddLogging(configure => configure.AddConsole());

        var serviceProvider = services.BuildServiceProvider();
        var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
        var logger = serviceProvider.GetRequiredService<ILogger<Program>>();

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
