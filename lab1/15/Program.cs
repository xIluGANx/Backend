using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Подключаем EF Core и Identity
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("MyWebAppDb")); // Для теста используем InMemory

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddControllersWithViews();
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login"; // маршрут для перенаправления неаутентифицированных
    options.AccessDeniedPath = "/Home/AccessDenied"; // маршрут для недостаточных прав
});

var app = builder.Build();

// Инициализация ролей и пользователей
using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    string[] roles = new[] { "Admin", "User" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole(role));
    }

    // Создаем тестовых пользователей
    var adminUser = await userManager.FindByNameAsync("admin@example.com");
    if (adminUser == null)
    {
        adminUser = new IdentityUser { UserName = "admin@example.com", Email = "admin@example.com" };
        await userManager.CreateAsync(adminUser, "Admin123!");
        await userManager.AddToRoleAsync(adminUser, "Admin");
    }

    var normalUser = await userManager.FindByNameAsync("user@example.com");
    if (normalUser == null)
    {
        normalUser = new IdentityUser { UserName = "user@example.com", Email = "user@example.com" };
        await userManager.CreateAsync(normalUser, "User123!");
        await userManager.AddToRoleAsync(normalUser, "User");
    }
}

// Подключаем middleware
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<RoleRedirectMiddleware>();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
