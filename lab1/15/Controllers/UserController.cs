using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize(Roles = "User,Admin")]
public class UserController : Controller
{
    public IActionResult Index()
    {
        return Content("Добро пожаловать, Пользователь!");
    }
}
