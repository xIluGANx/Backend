using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Prometheus;

var builder = WebApplication.CreateBuilder(args);

// ��������� MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Middleware Prometheus
app.UseHttpMetrics();

// ����������� �����
app.UseStaticFiles();
app.UseRouting();

// Endpoint ��� ������ Prometheus
app.UseEndpoints(endpoints =>
{
    endpoints.MapMetrics("/metrics"); // Prometheus �������� �������
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});

app.Run();
