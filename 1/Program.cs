using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Настройка сервисов приложения
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Настройка конвейера middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Добавление статических файлов (wwwroot)
app.UseStaticFiles();

// Определение маршрутов приложения
app.MapGet("/", () =>
{
    // Главная страница - возвращаем HTML с навигацией
    return Results.Text(
        """
        <!DOCTYPE html>
        <html>
        <head>
            <meta charset=utf-8>
            <title>Главная страница</title>
            <style>
                body { font-family: Arial, sans-serif; margin: 40px; }
                nav { margin-bottom: 20px; }
                a { margin-right: 15px; text-decoration: none; color: #007bff; }
                a:hover { text-decoration: underline; }
            </style>
        </head>
        <body>
            <nav>
                <a href="/">Главная</a>
                <a href="/about">О нас</a>
                <a href="/contact">Контакты</a>
                <a href="/api/data">API Данные</a>
            </nav>
            <h1>Добро пожаловать на главную страницу!</h1>
            <p>Это минимальное веб-приложение на ASP.NET Core с использованием класса WebApplication.</p>
        </body>
        </html>
        """, "text/html");
});

app.MapGet("/about", () =>
{
    // Страница "О нас"
    return Results.Text(
        """
        <!DOCTYPE html>
        <html>
        <head>
            <meta charset=utf-8>
            <title>О нас</title>
            <style>
                body { font-family: Arial, sans-serif; margin: 40px; }
                nav { margin-bottom: 20px; }
                a { margin-right: 15px; text-decoration: none; color: #007bff; }
                a:hover { text-decoration: underline; }
            </style>
        </head>
        <body>
            <nav>
                <a href="/">Главная</a>
                <a href="/about">О нас</a>
                <a href="/contact">Контакты</a>
                <a href="/api/data">API Данные</a>
            </nav>
            <h1>О нашей компании</h1>
            <p>Мы создаем современные веб-приложения на ASP.NET Core.</p>
            <ul>
                <li>Профессиональная команда</li>
                <li>Современные технологии</li>
                <li>Качественные решения</li>
            </ul>
        </body>
        </html>
        """, "text/html");
});

app.MapGet("/contact", () =>
{
    // Страница контактов
    return Results.Text(
        """
        <!DOCTYPE html>
        <html>
        <head>
            <meta charset=utf-8>
            <title>Контакты</title>
            <style>
                body { font-family: Arial, sans-serif; margin: 40px; }
                nav { margin-bottom: 20px; }
                a { margin-right: 15px; text-decoration: none; color: #007bff; }
                a:hover { text-decoration: underline; }
                .contact-info { background: #f8f9fa; padding: 20px; border-radius: 5px; }
            </style>
        </head>
        <body>
            <nav>
                <a href="/">Главная</a>
                <a href="/about">О нас</a>
                <a href="/contact">Контакты</a>
                <a href="/api/data">API Данные</a>
            </nav>
            <h1>Наши контакты</h1>
            <div class="contact-info">
                <p><strong>Email:</strong> info@example.com</p>
                <p><strong>Телефон:</strong> +7 (123) 456-78-90</p>
                <p><strong>Адрес:</strong> г. Москва, ул. Примерная, д. 123</p>
            </div>
        </body>
        </html>
        """, "text/html");
});

app.MapGet("/api/data", () =>
{
    // API endpoint возвращающий JSON данные
    var data = new
    {
        Message = "Добро пожаловать в наше API!",
        Timestamp = DateTime.Now,
        Version = "1.0",
        Features = new[] { "Минимальный API", "Маршрутизация", "JSON ответы" }
    };

    return Results.Json(data, new JsonSerializerOptions
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    });
});


// Запуск приложения
app.Run();
