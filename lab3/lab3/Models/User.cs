public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public int Age { get; set; }

    public override string ToString()
    {
        return $"User[Id={Id}, Name={Name}, Email={Email}, Age={Age}]";
    }
}