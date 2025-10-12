using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// Добавляем MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Middleware Prometheus
app.UseHttpMetrics();

// Статические файлы
app.UseStaticFiles();
app.UseRouting();

// Endpoint для метрик Prometheus
app.UseEndpoints(endpoints =>
{
    endpoints.MapMetrics("/metrics"); // Prometheus собирает метрики
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
