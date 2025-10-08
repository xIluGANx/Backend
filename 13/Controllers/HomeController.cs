using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{
	private readonly IAuthorizationService _authorizationService;

	public HomeController(IAuthorizationService authorizationService)
	{
		_authorizationService = authorizationService;
	}
	[AllowAnonymous] // Доступно без авторизации
    public IActionResult Index()
    {
        return View();
    }

    [Authorize] // Требуется любая авторизация
    public IActionResult Profile()
    {
        return View();
    }

    [Authorize(Roles = "Admin")] // Только для администраторов
    public IActionResult AdminPanel()
    {
        return View();
    }

    [Authorize(Roles = "User,Admin")] // Для пользователей и администраторов
    public IActionResult UserArea()
    {
        return View();
    }

    [Authorize(Policy = "AdminOnly")] // Использование политики
    public IActionResult AdminSettings()
    {
        return View();
    }
	[Authorize]
	public async Task<IActionResult> CheckPermissions()
	{
		var result = new
		{
			IsAuthenticated = User.Identity.IsAuthenticated,
			UserName = User.Identity.Name,
			IsInRoleAdmin = User.IsInRole("Admin"),
			IsInRoleUser = User.IsInRole("User"),
			CanAccessAdminPanel = (await _authorizationService.AuthorizeAsync(User, "AdminOnly")).Succeeded
		};

		return Json(result);
	}
}