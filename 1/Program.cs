using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// ��������� �������� ����������
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// ��������� ��������� middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// ���������� ����������� ������ (wwwroot)
app.UseStaticFiles();

// ����������� ��������� ����������
app.MapGet("/", () =>
{
    // ������� �������� - ���������� HTML � ����������
    return Results.Text(
        """
        <!DOCTYPE html>
        <html>
        <head>
            <meta charset=utf-8>
            <title>������� ��������</title>
            <style>
                body { font-family: Arial, sans-serif; margin: 40px; }
                nav { margin-bottom: 20px; }
                a { margin-right: 15px; text-decoration: none; color: #007bff; }
                a:hover { text-decoration: underline; }
            </style>
        </head>
        <body>
            <nav>
                <a href="/">�������</a>
                <a href="/about">� ���</a>
                <a href="/contact">��������</a>
                <a href="/api/data">API ������</a>
            </nav>
            <h1>����� ���������� �� ������� ��������!</h1>
            <p>��� ����������� ���-���������� �� ASP.NET Core � �������������� ������ WebApplication.</p>
        </body>
        </html>
        """, "text/html");
});

app.MapGet("/about", () =>
{
    // �������� "� ���"
    return Results.Text(
        """
        <!DOCTYPE html>
        <html>
        <head>
            <meta charset=utf-8>
            <title>� ���</title>
            <style>
                body { font-family: Arial, sans-serif; margin: 40px; }
                nav { margin-bottom: 20px; }
                a { margin-right: 15px; text-decoration: none; color: #007bff; }
                a:hover { text-decoration: underline; }
            </style>
        </head>
        <body>
            <nav>
                <a href="/">�������</a>
                <a href="/about">� ���</a>
                <a href="/contact">��������</a>
                <a href="/api/data">API ������</a>
            </nav>
            <h1>� ����� ��������</h1>
            <p>�� ������� ����������� ���-���������� �� ASP.NET Core.</p>
            <ul>
                <li>���������������� �������</li>
                <li>����������� ����������</li>
                <li>������������ �������</li>
            </ul>
        </body>
        </html>
        """, "text/html");
});

app.MapGet("/contact", () =>
{
    // �������� ���������
    return Results.Text(
        """
        <!DOCTYPE html>
        <html>
        <head>
            <meta charset=utf-8>
            <title>��������</title>
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
                <a href="/">�������</a>
                <a href="/about">� ���</a>
                <a href="/contact">��������</a>
                <a href="/api/data">API ������</a>
            </nav>
            <h1>���� ��������</h1>
            <div class="contact-info">
                <p><strong>Email:</strong> info@example.com</p>
                <p><strong>�������:</strong> +7 (123) 456-78-90</p>
                <p><strong>�����:</strong> �. ������, ��. ���������, �. 123</p>
            </div>
        </body>
        </html>
        """, "text/html");
});

app.MapGet("/api/data", () =>
{
    // API endpoint ������������ JSON ������
    var data = new
    {
        Message = "����� ���������� � ���� API!",
        Timestamp = DateTime.Now,
        Version = "1.0",
        Features = new[] { "����������� API", "�������������", "JSON ������" }
    };

    return Results.Json(data, new JsonSerializerOptions
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    });
});


// ������ ����������
app.Run();
