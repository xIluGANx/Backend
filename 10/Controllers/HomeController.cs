using Microsoft.AspNetCore.Mvc;

namespace ResponseDemo.Controllers
{
    public class HomeController : Controller
    {
        [Route("")]
        [Route("home")]
        [Route("home/index")]
        public IActionResult Index()
        {
            var menuItems = new[]
            {
                new { Url = "/api/response/json", Name = "JSON данные", Description = "Возврат структурированных данных в формате JSON" },
                new { Url = "/api/response/html", Name = "HTML контент", Description = "Динамически генерируемый HTML" },
                new { Url = "/api/response/file", Name = "Текстовый файл", Description = "Загрузка текстового файла" },
                new { Url = "/api/response/image", Name = "Изображение", Description = "Генерируемое изображение" },
                new { Url = "/api/response/csv", Name = "CSV данные", Description = "Данные в формате CSV" },
                new { Url = "/api/response/stream", Name = "Потоковые данные", Description = "Постепенная генерация контента" },
                new { Url = "/api/response/redirect", Name = "Перенаправление", Description = "Редирект на внешний сайт" },
                new { Url = "/api/response/redirect-internal", Name = "Внутренний редирект", Description = "Редирект внутри приложения" },
                new { Url = "/api/response/status/200", Name = "Статус 200", Description = "Успешный ответ" },
                new { Url = "/api/response/status/404", Name = "Статус 404", Description = "Ресурс не найден" },
                new { Url = "/api/response/status/500", Name = "Статус 500", Description = "Ошибка сервера" }
            };

            ViewBag.MenuItems = menuItems;
            return View();
        }
    }
}