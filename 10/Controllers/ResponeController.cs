using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace ResponseDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResponseController : ControllerBase
    {
        // 1. Возврат JSON данных
        [HttpGet("json")]
        public IActionResult GetJson()
        {
            var user = new
            {
                Id = 1,
                Name = "John Doe",
                Email = "john@example.com",
                CreatedAt = DateTime.UtcNow,
                Status = "active"
            };

            return Ok(user);
        }

        // 2. Возврат HTML контента
        [HttpGet("html")]
        public ContentResult GetHtml()
        {
            var htmlContent = @"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset=utf-8>
                    <title>HTML Response</title>
                    <style>
                        body { 
                            font-family: Arial, sans-serif; 
                            margin: 40px;
                            background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
                            color: white;
                        }
                        .container { 
                            max-width: 800px; 
                            margin: 0 auto; 
                            padding: 2rem;
                            background: rgba(255,255,255,0.1);
                            border-radius: 10px;
                            backdrop-filter: blur(10px);
                        }
                        h1 { color: #fff; text-align: center; }
                        ul { line-height: 1.8; }
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <h1>Демонстрация HTML ответа</h1>
                        <p>Этот HTML был сгенерирован в контроллере ASP.NET Core</p>
                        <ul>
                            <li>Динамическое содержимое</li>
                            <li>Стилизованное оформление</li>
                            <li>Генерация на сервере</li>
                        </ul>
                        <p><strong>Время генерации:</strong> " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + @"</p>
                    </div>
                </body>
                </html>";

            return Content(htmlContent, "text/html");
        }

        // 3. Возврат файла
        [HttpGet("file")]
        public IActionResult GetFile()
        {
            var fileContent = @"Это содержимое текстового файла.
Сгенерировано: " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + @"

Демонстрация работы с файлами в ASP.NET Core:
- Создание содержимого в памяти
- Установка MIME-типа
- Настройка имени файла

Спасибо за тестирование!";

            var byteArray = Encoding.UTF8.GetBytes(fileContent);
            var stream = new MemoryStream(byteArray);

            return File(stream, "text/plain", "example-file.txt");
        }

        // 4. Возврат файла изображения
        [HttpGet("image")]
        public IActionResult GetImage()
        {
            // Создаем простое изображение программно
            var width = 400;
            var height = 200;
            using var bitmap = new System.Drawing.Bitmap(width, height);

            using (var graphics = System.Drawing.Graphics.FromImage(bitmap))
            {
                // Градиентный фон
                var brush = new System.Drawing.Drawing2D.LinearGradientBrush(
                    new System.Drawing.Point(0, 0),
                    new System.Drawing.Point(width, height),
                    System.Drawing.Color.LightBlue,
                    System.Drawing.Color.DarkBlue);

                graphics.FillRectangle(brush, 0, 0, width, height);

                // Текст
                var font = new System.Drawing.Font("Arial", 16, System.Drawing.FontStyle.Bold);
                var textBrush = new System.Drawing.SolidBrush(System.Drawing.Color.White);
                graphics.DrawString("ASP.NET Core Response", font, textBrush, 50, 80);
                graphics.DrawString("Generated Image", font, textBrush, 100, 120);
            }

            var stream = new MemoryStream();
            bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            stream.Position = 0;

            return File(stream, "image/png", "generated-image.png");
        }

        // 5. Перенаправление
        [HttpGet("redirect")]
        public IActionResult RedirectExample()
        {
            return Redirect("https://dotnet.microsoft.com/");
        }

        [HttpGet("redirect-internal")]
        public IActionResult RedirectInternal()
        {
            return RedirectToAction("GetJson");
        }

        // 6. Возврат статусных кодов
        [HttpGet("status/{code:int}")]
        public IActionResult GetStatus(int code)
        {
            return code switch
            {
                200 => Ok(new
                {
                    message = "Запрос успешно обработан",
                    timestamp = DateTime.UtcNow,
                    data = new { userId = 123, status = "active" }
                }),
                201 => Created("/api/response/status/201", new
                {
                    message = "Ресурс создан",
                    id = 456,
                    location = "/api/resources/456"
                }),
                400 => BadRequest(new
                {
                    error = "Неверный запрос",
                    details = "Проверьте параметры запроса"
                }),
                404 => NotFound(new
                {
                    error = "Ресурс не найден",
                    requestedUrl = Request.Path,
                    suggestion = "Проверьте правильность URL"
                }),
                500 => StatusCode(500, new
                {
                    error = "Внутренняя ошибка сервера",
                    requestId = System.Diagnostics.Activity.Current?.Id
                }),
                _ => StatusCode(code, new
                {
                    message = $"Статус код: {code}",
                    description = "Пользовательский статус код"
                })
            };
        }

        // 7. Потоковые данные
        [HttpGet("stream")]
        public async Task<IActionResult> GetStream()
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);

            // Заголовок
            await writer.WriteLineAsync("=== Демонстрация потоковых данных ===");
            await writer.WriteLineAsync($"Начало: {DateTime.Now:HH:mm:ss}");
            await writer.WriteLineAsync();

            // Генерируем данные постепенно
            for (int i = 1; i <= 15; i++)
            {
                await writer.WriteLineAsync($"Запись #{i} - {DateTime.Now:HH:mm:ss.fff}");
                await writer.FlushAsync();
                await Task.Delay(300); // Имитация задержки обработки
            }

            // Завершение
            await writer.WriteLineAsync();
            await writer.WriteLineAsync($"Завершено: {DateTime.Now:HH:mm:ss}");
            await writer.WriteLineAsync($"Всего записей: 15");

            await writer.FlushAsync();
            stream.Position = 0;

            return File(stream, "text/plain", "stream-data.txt");
        }

        // 8. Пользовательский тип контента - CSV
        [HttpGet("csv")]
        public IActionResult GetCsv()
        {
            var csvData = new StringBuilder();
            csvData.AppendLine("Id,Name,Email,RegistrationDate,Status");
            csvData.AppendLine("1,Иван Иванов,ivan@example.com,2024-01-15,active");
            csvData.AppendLine("2,Петр Петров,petr@example.com,2024-01-16,active");
            csvData.AppendLine("3,Мария Сидорова,maria@example.com,2024-01-17,pending");
            csvData.AppendLine("4,Анна Козлова,anna@example.com,2024-01-18,active");
            csvData.AppendLine("5,Сергей Смирнов,sergey@example.com,2024-01-19,inactive");

            var byteArray = Encoding.UTF8.GetBytes(csvData.ToString());
            var stream = new MemoryStream(byteArray);

            return File(stream, "text/csv", "users-data.csv");
        }
    }
}