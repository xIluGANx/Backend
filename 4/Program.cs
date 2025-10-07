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

// ������������� �� ��� ����������� ��������
app.Use(async (context, next) =>
{
    Console.WriteLine($"��������� �������: {context.Request.Method} {context.Request.Path}");
    await next();
});

// ����������� ��������� ����������

// ������� ��������
app.MapGet("/", () =>
{
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
                .feature-list { margin: 20px 0; padding: 15px; background: #f8f9fa; border-radius: 5px; }
            </style>
        </head>
        <body>
            <nav>
                <a href="/">�������</a>
                <a href="/about">� ���</a>
                <a href="/contact">��������</a>
                <a href="/api/data">API ������</a>
                <a href="/products">������</a>
                <a href="/users/1">������� ������������</a>
            </nav>
            <h1>����� ���������� �� ������� ��������!</h1>
            <p>��� ����������� ���-���������� �� ASP.NET Core � �������������� ������ WebApplication.</p>
            
            <div class="feature-list">
                <h2>��������� ��������:</h2>
                <ul>
                    <li><strong>GET /</strong> - ������� ��������</li>
                    <li><strong>GET /about</strong> - �������� "� ���"</li>
                    <li><strong>GET /contact</strong> - �������� ���������</li>
                    <li><strong>GET /api/data</strong> - API ������ (JSON)</li>
                    <li><strong>GET /products</strong> - ������ �������</li>
                    <li><strong>GET /products/{id}</strong> - ���������� � ������ (ID: 1-1000)</li>
                    <li><strong>GET /users/{id}</strong> - ������� ������������ (ID: int)</li>
                    <li><strong>GET /search?query=�����</strong> - ����� �������</li>
                    <li><strong>POST /api/products</strong> - �������� ������ (JSON)</li>
                    <li><strong>GET /category/{categoryName}</strong> - ������ �� ���������</li>
                </ul>
            </div>
        </body>
        </html>
        """, "text/html");
});

// ����������� ��������
app.MapGet("/about", () =>
{
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
                <a href="/products">������</a>
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
                <a href="/products">������</a>
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

// API endpoints
app.MapGet("/api/data", () =>
{
    var data = new
    {
        Message = "����� ���������� � ���� API!",
        Timestamp = DateTime.Now,
        Version = "1.0",
        Features = new[] { "����������� API", "�������������", "JSON ������", "��������� ���������" }
    };

    return Results.Json(data, new JsonSerializerOptions
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    });
});

// ������� � ���������� � ������������ (������ ����� �� 1 �� 1000)
app.MapGet("/products/{id:int:range(1,1000)}", (int id) =>
{
    var product = new
    {
        Id = id,
        Name = $"����� {id}",
        Price = (id * 100) + 99.99m,
        Category = "�����������",
        Description = $"�������� ������ {id}",
        InStock = true
    };

    return Results.Json(product, new JsonSerializerOptions
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    });
});

// ������� ��� ������ �������
app.MapGet("/products", () =>
{
    var products = new[]
    {
        new { Id = 1, Name = "��������", Price = 29999.99m, Category = "�����������" },
        new { Id = 2, Name = "�������", Price = 59999.99m, Category = "�����������" },
        new { Id = 3, Name = "�����", Price = 599.99m, Category = "�����" },
        new { Id = 4, Name = "��������", Price = 4999.99m, Category = "�����������" }
    };

    return Results.Text(
        $$"""
        <!DOCTYPE html>
        <html>
        <head>
            <meta charset=utf-8>
            <title>������ �������</title>
            <style>
                body { font-family: Arial, sans-serif; margin: 40px; }
                nav { margin-bottom: 20px; }
                a { margin-right: 15px; text-decoration: none; color: #007bff; }
                a:hover { text-decoration: underline; }
                .product { border: 1px solid #ddd; padding: 15px; margin: 10px 0; border-radius: 5px; }
                .product h3 { margin-top: 0; }
            </style>
        </head>
        <body>
            <nav>
                <a href="/">�������</a>
                <a href="/about">� ���</a>
                <a href="/contact">��������</a>
                <a href="/api/data">API ������</a>
                <a href="/products">������</a>
            </nav>
            <h1>���� ������</h1>
            {{string.Join("", products.Select(p => $$"""
                <div class="product">
                    <h3>{{p.Name}}</h3>
                    <p><strong>����:</strong> {{p.Price}} ���.</p>
                    <p><strong>���������:</strong> {{p.Category}}</p>
                    <a href="/products/{{p.Id}}">���������</a>
                </div>
            """))}}
            <p>���������� �����: 
                <a href="/category/electronics">������ �����������</a> | 
                <a href="/category/books">�����</a>
            </p>
        </body>
        </html>
        """, "text/html");
});

// ������� � ���������� ��������� (��������� ��������)
app.MapGet("/category/{categoryName}", (string categoryName) =>
{
    var categories = new Dictionary<string, string>
    {
        { "electronics", "�����������" },
        { "books", "�����" }
    };

    if (categories.TryGetValue(categoryName.ToLower(), out var displayName))
    {
        return Results.Text(
            $$"""
            <!DOCTYPE html>
            <html>
            <head>
                <meta charset=utf-8>
                <title>���������: {{displayName}}</title>
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
                    <a href="/products">������</a>
                </nav>
                <h1>���������: {{displayName}}</h1>
                <p>����� ����� ������ ��������� "{{displayName}}"</p>
                <a href="/products">��������� �� ���� �������</a>
            </body>
            </html>
            """, "text/html");
    }
    else
    {
        return Results.NotFound($"��������� '{categoryName}' �� �������");
    }
});

// ������� � ���������� ������� (query parameter)
app.MapGet("/search", (string query) =>
{
    if (string.IsNullOrEmpty(query))
    {
        return Results.BadRequest("�� ������ �������� ������");
    }

    var results = new
    {
        Query = query,
        Results = new[]
        {
            new { Id = 1, Name = $"��������� 1 �� ������� '{query}'", Relevance = 0.95 },
            new { Id = 2, Name = $"��������� 2 �� ������� '{query}'", Relevance = 0.87 },
            new { Id = 3, Name = $"��������� 3 �� ������� '{query}'", Relevance = 0.76 }
        },
        TotalCount = 3,
        SearchTime = DateTime.Now
    };

    return Results.Json(results, new JsonSerializerOptions
    {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    });
});

// ������� ��� ������� ������������ � ������������ (������ ����� �����)
app.MapGet("/users/{id:int}", (int id) =>
{
    var user = new
    {
        Id = id,
        Username = $"user{id}",
        Email = $"user{id}@example.com",
        RegistrationDate = DateTime.Now.AddDays(-id * 10),
        IsActive = true
    };

    return Results.Text(
        $$"""
        <!DOCTYPE html>
        <html>
        <head>
            <meta charset=utf-8>
            <title>������� ������������ {{user.Username}}</title>
            <style>
                body { font-family: Arial, sans-serif; margin: 40px; }
                nav { margin-bottom: 20px; }
                a { margin-right: 15px; text-decoration: none; color: #007bff; }
                a:hover { text-decoration: underline; }
                .profile { background: #f8f9fa; padding: 20px; border-radius: 5px; }
            </style>
        </head>
        <body>
            <nav>
                <a href="/">�������</a>
                <a href="/about">� ���</a>
                <a href="/contact">��������</a>
                <a href="/api/data">API ������</a>
                <a href="/products">������</a>
            </nav>
            <h1>������� ������������</h1>
            <div class="profile">
                <p><strong>ID:</strong> {{user.Id}}</p>
                <p><strong>��� ������������:</strong> {{user.Username}}</p>
                <p><strong>Email:</strong> {{user.Email}}</p>
                <p><strong>���� �����������:</strong> {{user.RegistrationDate:dd.MM.yyyy}}</p>
                <p><strong>������:</strong> {{(user.IsActive ? "�������" : "���������")}}</p>
            </div>
        </body>
        </html>
        """, "text/html");
});

// POST ������� ��� �������� ������
app.MapPost("/api/products", async (HttpContext context) =>
{
    try
    {
        // ������ ���� �������
        using var reader = new StreamReader(context.Request.Body, Encoding.UTF8);
        var requestBody = await reader.ReadToEndAsync();

        // �������������� JSON
        var product = JsonSerializer.Deserialize<Product>(requestBody, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        if (product == null)
        {
            return Results.BadRequest("�������� ������ ������");
        }

        // �������� ���������� � ���� ������
        product.Id = new Random().Next(1000, 10000);
        product.CreatedAt = DateTime.Now;

        return Results.Json(new
        {
            Message = "����� ������� ������",
            Product = product
        }, new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });
    }
    catch (JsonException)
    {
        return Results.BadRequest("������ � ������� JSON");
    }
});

// ��������� �������������� ���������
app.MapFallback(() =>
{
    return Results.Text(
        """
        <!DOCTYPE html>
        <html>
        <head>
            <meta charset=utf-8>
            <title>�������� �� �������</title>
            <style>
                body { font-family: Arial, sans-serif; margin: 40px; text-align: center; }
                nav { margin-bottom: 20px; }
                a { margin-right: 15px; text-decoration: none; color: #007bff; }
                a:hover { text-decoration: underline; }
                .error { color: #dc3545; }
            </style>
        </head>
        <body>
            <nav>
                <a href="/">�������</a>
                <a href="/about">� ���</a>
                <a href="/contact">��������</a>
                <a href="/api/data">API ������</a>
                <a href="/products">������</a>
            </nav>
            <h1 class="error">404 - �������� �� �������</h1>
            <p>����������� �������� �� ����������.</p>
            <p><a href="/">��������� �� ������� ��������</a></p>
        </body>
        </html>
        """, "text/html");
});

// ������ ����������
app.Run();
// ������ ������ ��� ������ ������ ���� ��������� ����� ����� ����������� ����������
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Category { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
}