using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


// Создание и настройка хоста
var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        // Конфигурация приложения
        var configuration = context.Configuration;
        
        // Регистрация сервисов с различными временами жизни
        
        // Transient - новый экземпляр при каждом запросе
        services.AddTransient<IDataService, DataService>();
        services.AddTransient<IEmailService, EmailService>();
        
        // Scoped - один экземпляр на область видимости (в консольном приложении ведет себя как Singleton)
        services.AddScoped<IUserRepository, UserRepository>();
        
        // Singleton - один экземпляр на все приложение
        services.AddSingleton<IReportGenerator, ReportGenerator>();
        
        // Регистрация основного приложения
        services.AddTransient<Application>();
        
        // Конфигурация сервисов через options pattern
        services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));
        services.Configure<DataSettings>(configuration.GetSection("DataSettings"));
        
        // Логирование уже настроено в CreateDefaultBuilder
        
    })
    .ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.AddConsole();
        logging.SetMinimumLevel(LogLevel.Information);
    })
    .Build();

// Запуск приложения
try
{
    using var scope = host.Services.CreateScope();
    var app = scope.ServiceProvider.GetRequiredService<Application>();
    await app.RunAsync();
}
catch (Exception ex)
{
    var logger = host.Services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "Произошла ошибка при запуске приложения");
}