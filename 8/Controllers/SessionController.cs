using Microsoft.AspNetCore.Mvc;

namespace MyWebApp.Controllers
{
    public class SessionController : Controller
    {
        // Главная страница
        public IActionResult Index()
        {
            return PhysicalFile("wwwroot/index.html", "text/html");
        }

        // Сессия на сервере
        public IActionResult SetSession()
        {
            HttpContext.Session.SetString("Username", "Alice");
            return Content("Session value set!");
        }

        public IActionResult GetSession()
        {
            var username = HttpContext.Session.GetString("Username") ?? "No value";
            return Content($"Session value: {username}");
        }

        // Cookies на клиенте
        public IActionResult SetCookie()
        {
            Response.Cookies.Append("UserToken", "123456", new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddMinutes(60)
            });
            return Content("Cookie set!");
        }

        public IActionResult GetCookie()
        {
            var value = Request.Cookies["UserToken"] ?? "No cookie";
            return Content($"Cookie value: {value}");
        }
    }
}
