using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);

// ВАЖНО: Data Protection ДО аутентификации
builder.Services.AddDataProtection()
    .SetApplicationName("AuthExample");

// Настройка аутентификации
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
{
    options.Cookie.Name = "AuthCookie";
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.SlidingExpiration = true;

    // Критически важные настройки
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.None; // Для HTTP
    options.Cookie.SameSite = SameSiteMode.Lax;
    options.Cookie.Path = "/";

    // Отладка
    options.Events = new CookieAuthenticationEvents
    {
        OnSigningIn = context =>
        {
            Console.WriteLine($"🔐 SIGNING IN: {context.Principal.Identity.Name}");
            return Task.CompletedTask;
        },
        OnSignedIn = context =>
        {
            Console.WriteLine($"✅ SIGNED IN: {context.Principal.Identity.Name}");
            return Task.CompletedTask;
        },
        OnValidatePrincipal = context =>
        {
            Console.WriteLine($"🔍 VALIDATING: {context.Principal?.Identity?.Name}");
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

// Конфигурация pipeline
app.UseStaticFiles();
app.UseRouting();

// ВАЖНО: Правильный порядок!
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();