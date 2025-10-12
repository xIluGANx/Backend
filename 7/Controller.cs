using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class WeatherController : ControllerBase
{
    private readonly ILogger<WeatherController> _logger;

    public WeatherController(ILogger<WeatherController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<string> Get()
    {
        _logger.LogTrace("Начало метода Get");
        _logger.LogInformation("Запрос погоды получен");
        _logger.LogWarning("Используются устаревшие данные");
        _logger.LogError("Ошибка при получении данных");
        _logger.LogCritical("Сбой сервиса погоды");

        return new[] { "Sunny", "Cloudy", "Rainy" };
    }
}
