using Microsoft.AspNetCore.Mvc;
using Prometheus;

public class HomeController : Controller
{
    // Счетчик запросов к Home/Index
    private static readonly Counter IndexCounter =
        Metrics.CreateCounter("home_index_requests_total", "Количество запросов к Home/Index");

    public IActionResult Index()
    {
        IndexCounter.Inc(); // Увеличиваем счетчик при каждом запросе
        return View();
    }

    public IActionResult Error()
    {
        throw new System.Exception("Тестовое исключение для мониторинга");
    }
}
