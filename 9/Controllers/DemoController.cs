using Microsoft.AspNetCore.Mvc;

public class DemoController : Controller
{
    public IActionResult Index()
    {
        throw new Exception("Тестовое исключение");
    }

    public IActionResult NotFoundTest()
    {
        return NotFound(); // Генерация ошибки 404
    }
}
