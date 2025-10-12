using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return Content("Главная страница приложения");
    }

    public IActionResult AccessDenied()
    {
        return Content("Доступ запрещен!");
    }
}
