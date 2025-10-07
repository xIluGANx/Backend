using Microsoft.EntityFrameworkCore;
using EFCoreApp.Data;
using EFCoreApp.Repositories;

var builder = WebApplication.CreateBuilder(args);

// ���������� �������� � ���������
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ��������� Entity Framework Core � SQL Server
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ����������� ������������
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// ��������� CORS ��� �����-�������� ��������
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()  // ��������� ������� � ������ ������
              .AllowAnyMethod()  // ��������� ����� HTTP ������ (GET, POST, PUT, DELETE � �.�.)
              .AllowAnyHeader(); // ��������� ����� ���������
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

// ��������� ��������� HTTP ��������
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    // ������������� ���� ������ � ���������� ������� � development
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated(); // ������� �� ���� �� ����������
}

app.UseHttpsRedirection();

// ������������� CORS ��������
app.UseCors("AllowAll"); // ��������� �������� CORS �� ���� ��������

app.UseAuthorization();
app.MapControllers();

app.Run();