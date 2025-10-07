using Microsoft.Extensions.Logging;

public class UserRepository : IUserRepository
{
    private readonly ILogger<UserRepository> _logger;
    private readonly List<User> _users;

    public UserRepository(ILogger<UserRepository> logger)
    {
        _logger = logger;
        _users = new List<User>
        {
            new User { Id = 1, Name = "Иван Иванов", Email = "ivan@example.com", Age = 30 },
            new User { Id = 2, Name = "Петр Петров", Email = "petr@example.com", Age = 25 },
            new User { Id = 3, Name = "Мария Сидорова", Email = "maria@example.com", Age = 28 }
        };

        _logger.LogInformation("UserRepository создан с {Count} пользователями", _users.Count);
    }

    public async Task<User> GetUserByIdAsync(int id)
    {
        _logger.LogInformation("Поиск пользователя с ID: {Id}", id);
        await Task.Delay(100);

        return _users.FirstOrDefault(u => u.Id == id);
    }

    public async Task<List<User>> GetAllUsersAsync()
    {
        _logger.LogInformation("Получение всех пользователей");
        await Task.Delay(150);

        return _users;
    }

    public async Task<bool> AddUserAsync(User user)
    {
        _logger.LogInformation("Добавление пользователя: {Name}", user.Name);
        await Task.Delay(80);

        user.Id = _users.Max(u => u.Id) + 1;
        _users.Add(user);
        return true;
    }

    public async Task<bool> UpdateUserAsync(User user)
    {
        _logger.LogInformation("Обновление пользователя с ID: {Id}", user.Id);
        await Task.Delay(80);

        var existingUser = _users.FirstOrDefault(u => u.Id == user.Id);
        if (existingUser != null)
        {
            existingUser.Name = user.Name;
            existingUser.Email = user.Email;
            existingUser.Age = user.Age;
            return true;
        }

        return false;
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        _logger.LogInformation("Удаление пользователя с ID: {Id}", id);
        await Task.Delay(80);

        var user = _users.FirstOrDefault(u => u.Id == id);
        if (user != null)
        {
            _users.Remove(user);
            return true;
        }

        return false;
    }
}