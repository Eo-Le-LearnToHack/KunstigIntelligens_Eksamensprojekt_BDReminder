using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BirthdayReminder.Data;

var builder = WebApplication.CreateBuilder(args);

// Tilføj services til containeren
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); // Konfigurerer DbContext til at bruge SQL Server

// Tilføj Identity services med roller
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>() // Tilføj rollebaseret autorisation
    .AddEntityFrameworkStores<ApplicationDbContext>(); // Konfigurerer Entity Framework til at bruge ApplicationDbContext

// Konfigurer cookie-indstillinger, herunder Access Denied sti
builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/Shared/AccessDenied"; // Indstil stien til Access Denied-siden
});

builder.Services.AddControllersWithViews(); // Tilføj MVC Controller og View support

var app = builder.Build();

// Konfigurer HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage(); // Brug udviklerspecifik fejlside i udviklingsmiljøet
}
else
{
    app.UseExceptionHandler("/Home/Error"); // Brug brugerdefineret fejlside i produktionsmiljøet
    app.UseHsts(); // Brug HTTP Strict Transport Security i produktionsmiljøet
}

app.UseHttpsRedirection(); // Omdiriger HTTP til HTTPS
app.UseStaticFiles(); // Tillad servering af statiske filer

app.UseRouting(); // Aktiver routing

app.UseAuthentication(); // Aktiver autentifikation
app.UseAuthorization(); // Aktiver autorisation

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); // Definer standard routing

app.MapRazorPages(); // Map Razor Pages

// Initialiser roller og admin-bruger
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var userManager = services.GetRequiredService<UserManager<IdentityUser>>(); // Hent UserManager service
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>(); // Hent RoleManager service
    RoleInitializer.InitializeAsync(userManager, roleManager).Wait(); // Initialiser roller
    SeedData.Initialize(services).Wait(); // Initialiser seed data, inklusiv admin-bruger
}

app.Run(); // Kør applikationen
