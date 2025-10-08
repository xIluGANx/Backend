public interface IUserService
{
    User Authenticate(string username, string password);
}

public class UserService : IUserService
{
    // В реальном приложении - база данных
    private readonly List<User> _users = new()
    {
        new User { Id = 1, Username = "admin", Password = "admin123", Role = "Admin" },
        new User { Id = 2, Username = "user", Password = "user123", Role = "User" }
    };

    public User Authenticate(string username, string password)
    {
        return _users.FirstOrDefault(x =>
            x.Username == username && x.Password == password);
    }
}