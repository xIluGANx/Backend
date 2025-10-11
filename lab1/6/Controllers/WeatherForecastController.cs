using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using WebApplicationConfig.Configuration;
using WebApplicationConfig.Services;

namespace WebApplicationConfig.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly ApplicationSettings _appSettings;
    private readonly FeatureFlags _featureFlags;
    private readonly IConfigurationService _configService;

    public WeatherForecastController(
        ILogger<WeatherForecastController> logger,
        IOptions<ApplicationSettings> appSettings,
        IOptions<FeatureFlags> featureFlags,
        IConfigurationService configService)
    {
        _logger = logger;
        _appSettings = appSettings.Value;
        _featureFlags = featureFlags.Value;
        _configService = configService;
    }

    [HttpGet]
    public IActionResult Get()
    {
        // Логирование в зависимости от конфигурации
        if (_appSettings.DebugMode)
        {
            _logger.LogInformation("WeatherForecast endpoint called in debug mode");
        }

        var forecasts = Enumerable.Range(1, _appSettings.MaxItemsPerPage).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();

        // Добавляем информацию о конфигурации в ответ
        var response = new
        {
            Environment = _appSettings.Environment,
            AppName = _appSettings.AppName,
            MaxItems = _appSettings.MaxItemsPerPage,
            Forecasts = forecasts
        };

        return Ok(response);
    }

    [HttpGet("config")]
    public IActionResult GetConfig()
    {
        var configInfo = _configService.GetConfigurationInfo();
        return Ok(configInfo);
    }

    [HttpGet("features")]
    public IActionResult GetFeatures()
    {
        return Ok(_featureFlags);
    }
}

public record WeatherForecast
{
    public DateOnly Date { get; set; }
    public int TemperatureC { get; set; }
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
    public string? Summary { get; set; }
}