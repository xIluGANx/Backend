using Microsoft.AspNetCore.Mvc;
using WebApplicationConfig.Configuration;
using WebApplicationConfig.Services;
using Swashbuckle.AspNetCore.SwaggerGen;

var builder = WebApplication.CreateBuilder(args);

// Настройка конфигурации из различных источников
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables() // Добавляем переменные среды
    .AddCommandLine(args);     // Добавляем аргументы командной строки

// Регистрация сервисов
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Конфигурация Swagger на основе FeatureFlags
var featureFlags = builder.Configuration.GetSection("FeatureFlags").Get<FeatureFlags>();
if (featureFlags?.EnableSwagger == true)
{
    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new() { Title = "WebApplicationConfig API", Version = "v1" });
    });
}

// Регистрация кастомных сервисов
builder.Services.Configure<ApplicationSettings>(builder.Configuration.GetSection("ApplicationSettings"));
builder.Services.Configure<FeatureFlags>(builder.Configuration.GetSection("FeatureFlags"));
builder.Services.AddSingleton<IConfigurationService, ConfigurationService>();

var app = builder.Build();

// Конфигурация pipeline в зависимости от среды
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

if (featureFlags?.EnableSwagger == true)
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WebApplicationConfig v1"));
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Endpoint для отображения текущей конфигурации
app.MapGet("/config", (IConfiguration config, IConfigurationService configService) =>
{
    return configService.GetConfigurationInfo();
});

app.Run();