using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MyCachingApp.Services;

var builder = WebApplication.CreateBuilder(args);

// In-Memory Cache
builder.Services.AddMemoryCache();

// Регистрируем сервисы
builder.Services.AddSingleton<WeatherService>();
builder.Services.AddSingleton<FileCacheService>();

builder.Services.AddControllers();
builder.Services.AddResponseCaching();

var app = builder.Build();

app.UseResponseCaching();
app.MapControllers();
app.Run();
