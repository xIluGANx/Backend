using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;
    private readonly EmailSettings _settings;

    public EmailService(ILogger<EmailService> logger, IOptions<EmailSettings> options)
    {
        _logger = logger;
        _settings = options.Value;
        _logger.LogInformation("EmailService создан с SMTP сервером: {SmtpServer}", _settings.SmtpServer);
    }

    public async Task<bool> SendEmailAsync(string to, string subject, string body)
    {
        _logger.LogInformation("Отправка email пользователю: {To}, Тема: {Subject}", to, subject);

        // Имитация отправки email
        await Task.Delay(200);

        var success = !string.IsNullOrEmpty(to) && to.Contains("@");
        if (success)
        {
            _logger.LogInformation("Email успешно отправлен");
        }
        else
        {
            _logger.LogWarning("Не удалось отправить email");
        }

        return success;
    }

    public async Task<bool> ValidateEmailAsync(string email)
    {
        _logger.LogInformation("Валидация email: {Email}", email);
        await Task.Delay(50);

        return !string.IsNullOrEmpty(email) &&
               email.Contains("@") &&
               email.Contains(".");
    }
}