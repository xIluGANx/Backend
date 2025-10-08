using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

public class AccountController : Controller
{
    private readonly IUserService _userService;

    public AccountController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login()
    {
        Console.WriteLine("📄 Login page loaded");
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginModel model)
    {
        Console.WriteLine($"🔐 Login attempt: {model.Username}");

        if (!ModelState.IsValid)
        {
            Console.WriteLine("❌ Model state invalid");
            return View(model);
        }

        var user = _userService.Authenticate(model.Username, model.Password);

        if (user == null)
        {
            Console.WriteLine("❌ User not found or wrong password");
            ModelState.AddModelError("", "Неверные учетные данные");
            return View(model);
        }

        Console.WriteLine($"✅ User authenticated: {user.Username}, Role: {user.Role}");

        // Создаем claims
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role),
            new Claim("AuthenticatedAt", DateTime.Now.ToString())
        };

        var claimsIdentity = new ClaimsIdentity(
            claims,
            CookieAuthenticationDefaults.AuthenticationScheme);

        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30),
            IssuedUtc = DateTimeOffset.UtcNow,
            AllowRefresh = true
        };

        try
        {
            Console.WriteLine("🚀 Attempting SignInAsync...");

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            Console.WriteLine($"🎉 SignInAsync SUCCESS for {user.Username}");

            // Проверяем сразу
            var authResult = await HttpContext.AuthenticateAsync();
            Console.WriteLine($"📊 Immediate auth check: {authResult.Succeeded}");

            return RedirectToAction("Index", "Home");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"💥 SignInAsync ERROR: {ex.Message}");
            ModelState.AddModelError("", "Ошибка при входе в систему");
            return View(model);
        }
    }

    [HttpPost]
    [Authorize]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        Console.WriteLine("👋 User logged out");
        return RedirectToAction("Index", "Home");
    }

    [HttpGet]
    [AllowAnonymous]
    public IActionResult AccessDenied()
    {
        return View();
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> DebugAuth()
    {
        var authResult = await HttpContext.AuthenticateAsync();

        var debugInfo = new
        {
            User = new
            {
                IsAuthenticated = User.Identity.IsAuthenticated,
                UserName = User.Identity.Name,
                AuthenticationType = User.Identity.AuthenticationType
            },
            AuthenticationResult = new
            {
                Succeeded = authResult.Succeeded,
                Principal = authResult.Principal != null ? new
                {
                    IsAuthenticated = authResult.Principal.Identity.IsAuthenticated,
                    Name = authResult.Principal.Identity.Name
                } : null,
                Failure = authResult.Failure?.Message
            },
            Cookies = Request.Cookies.Select(c => new { c.Key, ValueLength = c.Value?.Length }).ToList(),
            Headers = Request.Headers.Where(h => h.Key.StartsWith("Cookie")).ToDictionary(h => h.Key, h => h.Value)
        };

        return Json(debugInfo);
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> TestLogin()
    {
        // Тестовый метод для прямого входа
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, "999"),
            new Claim(ClaimTypes.Name, "testuser"),
            new Claim(ClaimTypes.Role, "Admin"),
            new Claim("Test", "DirectLogin")
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(claimsIdentity),
            new AuthenticationProperties { IsPersistent = true });

        return RedirectToAction("Index", "Home");
    }
}