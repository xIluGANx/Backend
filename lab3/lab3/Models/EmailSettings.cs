public class EmailSettings
{
    public string SmtpServer { get; set; } = "smtp.example.com";
    public int Port { get; set; } = 587;
    public string Username { get; set; } = "username";
    public string Password { get; set; } = "password";
}