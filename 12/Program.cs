using Microsoft.EntityFrameworkCore;
using EFCoreApp.Data;
using EFCoreApp.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Добавление сервисов в контейнер
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Настройка Entity Framework Core с SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Регистрация репозиториев
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Настройка CORS для кросс-доменных запросов
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()  // Разрешить запросы с любого домена
              .AllowAnyMethod()  // Разрешить любые HTTP методы (GET, POST, PUT, DELETE и т.д.)
              .AllowAnyHeader(); // Разрешить любые заголовки
    });

    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "https://myapp.com")
              .WithMethods("GET", "POST", "PUT", "DELETE")
              .WithHeaders("Content-Type", "Authorization")
              .AllowCredentials();
    });
});

var app = builder.Build();

// Настройка конвейера HTTP запросов
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // Инициализация базы данных с начальными данными в development
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated(); // Создает БД если не существует
}

app.UseHttpsRedirection();

// Использование CORS политики
app.UseCors("AllowAll"); // Применяем политику CORS ко всем запросам

app.UseAuthorization();
app.MapControllers();

app.Run();