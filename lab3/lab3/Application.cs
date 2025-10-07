using Microsoft.Extensions.Logging;

public class Application
{
    private readonly IDataService _dataService;
    private readonly IEmailService _emailService;
    private readonly IUserRepository _userRepository;
    private readonly IReportGenerator _reportGenerator;
    private readonly ILogger<Application> _logger;

    // Внедрение зависимостей через конструктор
    public Application(
        IDataService dataService,
        IEmailService emailService,
        IUserRepository userRepository,
        IReportGenerator reportGenerator,
        ILogger<Application> logger)
    {
        _dataService = dataService;
        _emailService = emailService;
        _userRepository = userRepository;
        _reportGenerator = reportGenerator;
        _logger = logger;

        _logger.LogInformation("Application создан с внедренными зависимостями");
    }

    public async Task RunAsync()
    {
        _logger.LogInformation("Запуск приложения");

        Console.WriteLine("=== КОНСОЛЬНОЕ ПРИЛОЖЕНИЕ С DI ===");
        Console.WriteLine();

        try
        {
            // Демонстрация работы с пользователями
            await DemonstrateUserOperations();

            // Демонстрация работы с данными
            await DemonstrateDataOperations();

            // Демонстрация отправки email
            await DemonstrateEmailOperations();

            // Демонстрация генерации отчетов
            await DemonstrateReportGeneration();

            Console.WriteLine();
            Console.WriteLine("Все операции завершены успешно!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при выполнении приложения");
            Console.WriteLine($"Произошла ошибка: {ex.Message}");
        }
    }

    private async Task DemonstrateUserOperations()
    {
        Console.WriteLine("1. ОПЕРАЦИИ С ПОЛЬЗОВАТЕЛЯМИ");
        Console.WriteLine("=============================");

        // Получение всех пользователей
        var users = await _userRepository.GetAllUsersAsync();
        Console.WriteLine($"Найдено пользователей: {users.Count}");

        // Добавление нового пользователя
        var newUser = new User { Name = "Алексей Новый", Email = "alexey@example.com", Age = 35 };
        await _userRepository.AddUserAsync(newUser);
        Console.WriteLine($"Добавлен пользователь: {newUser.Name}");

        // Получение пользователя по ID
        var user = await _userRepository.GetUserByIdAsync(1);
        Console.WriteLine($"Найден пользователь: {user}");

        Console.WriteLine();
    }

    private async Task DemonstrateDataOperations()
    {
        Console.WriteLine("2. ОПЕРАЦИИ С ДАННЫМИ");
        Console.WriteLine("======================");

        // Получение данных
        var data = await _dataService.GetDataAsync();
        Console.WriteLine($"Получено элементов данных: {data.Count}");

        // Обработка данных
        var processed = await _dataService.ProcessDataAsync("тестовые данные");
        Console.WriteLine($"Обработанные данные: {processed}");

        // Сохранение новых данных
        await _dataService.SaveDataAsync("Новые данные из консоли");
        Console.WriteLine("Данные сохранены");

        Console.WriteLine();
    }

    private async Task DemonstrateEmailOperations()
    {
        Console.WriteLine("3. ОПЕРАЦИИ С EMAIL");
        Console.WriteLine("====================");

        // Валидация email
        var isValid = await _emailService.ValidateEmailAsync("test@example.com");
        Console.WriteLine($"Email валиден: {isValid}");

        // Отправка email
        var sent = await _emailService.SendEmailAsync(
            "user@example.com",
            "Тестовое сообщение",
            "Это тестовое сообщение из консольного приложения");

        Console.WriteLine($"Email отправлен: {sent}");

        Console.WriteLine();
    }

    private async Task DemonstrateReportGeneration()
    {
        Console.WriteLine("4. ГЕНЕРАЦИЯ ОТЧЕТОВ");
        Console.WriteLine("=====================");

        // Получение данных для отчетов
        var users = await _userRepository.GetAllUsersAsync();
        var data = await _dataService.GetDataAsync();

        // Генерация отчетов
        var userReport = await _reportGenerator.GenerateUserReportAsync(users);
        var dataReport = await _reportGenerator.GenerateDataReportAsync(data);

        Console.WriteLine("Отчет о пользователях:");
        Console.WriteLine(userReport);

        Console.WriteLine("Отчет о данных:");
        Console.WriteLine(dataReport);
    }
}