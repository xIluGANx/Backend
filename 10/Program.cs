var builder = WebApplication.CreateBuilder(args);

// Добавляем сервисы MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Разрешаем использование статических файлов
app.UseStaticFiles();

// Включаем маршрутизацию
app.UseRouting();

// Настраиваем маршруты
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();