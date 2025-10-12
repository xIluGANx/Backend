using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// ���������� MVC
builder.Services.AddControllersWithViews();

// ���������� ��� ��� ������ � ������
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Session}/{action=Index}/{id?}");

app.Run();
