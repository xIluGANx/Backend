
using Microsoft.Extensions.Logging;
using System.Text;

public class ReportGenerator : IReportGenerator
{
    private readonly ILogger<ReportGenerator> _logger;
    public string ReportType => "Подробный отчет";

    public ReportGenerator(ILogger<ReportGenerator> logger)
    {
        _logger = logger;
        _logger.LogInformation("ReportGenerator создан с типом: {ReportType}", ReportType);
    }

    public async Task<string> GenerateUserReportAsync(List<User> users)
    {
        _logger.LogInformation("Генерация отчета для {Count} пользователей", users.Count);
        await Task.Delay(300);

        var report = new StringBuilder();
        report.AppendLine("=== ОТЧЕТ О ПОЛЬЗОВАТЕЛЯХ ===");
        report.AppendLine($"Дата генерации: {DateTime.Now}");
        report.AppendLine($"Количество пользователей: {users.Count}");
        report.AppendLine();

        foreach (var user in users)
        {
            report.AppendLine($"ID: {user.Id}");
            report.AppendLine($"Имя: {user.Name}");
            report.AppendLine($"Email: {user.Email}");
            report.AppendLine($"Возраст: {user.Age}");
            report.AppendLine("---");
        }

        report.AppendLine("=== КОНЕЦ ОТЧЕТА ===");

        return report.ToString();
    }

    public async Task<string> GenerateDataReportAsync(List<string> data)
    {
        _logger.LogInformation("Генерация отчета для {Count} элементов данных", data.Count);
        await Task.Delay(200);

        var report = new StringBuilder();
        report.AppendLine("=== ОТЧЕТ О ДАННЫХ ===");
        report.AppendLine($"Дата генерации: {DateTime.Now}");
        report.AppendLine($"Количество элементов: {data.Count}");
        report.AppendLine();

        for (int i = 0; i < data.Count; i++)
        {
            report.AppendLine($"{i + 1}. {data[i]}");
        }

        report.AppendLine("=== КОНЕЦ ОТЧЕТА ===");

        return report.ToString();
    }
}