using Microsoft.Extensions.Logging;

public class DataService : IDataService
{
    private readonly ILogger<DataService> _logger;
    private readonly List<string> _mockData;

    public DataService(ILogger<DataService> logger)
    {
        _logger = logger;
        _mockData = new List<string>
        {
            "Данные 1", "Данные 2", "Данные 3", "Данные 4", "Данные 5"
        };

        _logger.LogInformation("DataService создан");
    }

    public async Task<List<string>> GetDataAsync()
    {
        _logger.LogInformation("Получение данных из DataService");
        await Task.Delay(100); // Имитация асинхронной операции
        return _mockData;
    }

    public async Task<bool> SaveDataAsync(string data)
    {
        _logger.LogInformation("Сохранение данных: {Data}", data);
        await Task.Delay(50);
        _mockData.Add(data);
        return true;
    }

    public async Task<string> ProcessDataAsync(string input)
    {
        _logger.LogInformation("Обработка данных: {Input}", input);
        await Task.Delay(80);
        return $"Обработанные: {input.ToUpper()}";
    }
}