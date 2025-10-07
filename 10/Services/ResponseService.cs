namespace ResponseDemo.Services;
using System.Text;

public class ResponseService : IResponseService
{
    public async Task<byte[]> GenerateSampleImageAsync()
    {
        // Генерация изображения
        using var bitmap = new System.Drawing.Bitmap(200, 100);
        using var graphics = System.Drawing.Graphics.FromImage(bitmap);

        graphics.Clear(System.Drawing.Color.LightBlue);
        graphics.DrawString("ASP.NET Core",
            new System.Drawing.Font("Arial", 12),
            System.Drawing.Brushes.DarkBlue,
            new System.Drawing.PointF(10, 40));

        using var stream = new MemoryStream();
        bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
        return stream.ToArray();
    }

    public async Task<string> GenerateCsvDataAsync()
    {
        var csvBuilder = new StringBuilder();
        csvBuilder.AppendLine("Id,Name,Email,RegistrationDate");

        for (int i = 1; i <= 5; i++)
        {
            csvBuilder.AppendLine($"{i},User {i},user{i}@example.com,{DateTime.Now:yyyy-MM-dd}");
        }

        return csvBuilder.ToString();
    }

    public async Task<Stream> GenerateStreamDataAsync()
    {
        var stream = new MemoryStream();
        var writer = new StreamWriter(stream);

        for (int i = 1; i <= 10; i++)
        {
            await writer.WriteLineAsync($"Record {i}: {DateTime.Now:HH:mm:ss.fff}");
            await writer.FlushAsync();
            await Task.Delay(200);
        }

        stream.Position = 0;
        return stream;
    }
}