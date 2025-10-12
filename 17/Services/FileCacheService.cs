using System;
using System.IO;
using System.Threading.Tasks;

namespace MyCachingApp.Services
{
    public class FileCacheService
    {
        private readonly string _cacheFolder = "CacheFiles";
        private readonly string _cacheFile;

        public FileCacheService()
        {
            if (!Directory.Exists(_cacheFolder))
                Directory.CreateDirectory(_cacheFolder);

            _cacheFile = Path.Combine(_cacheFolder, "weather.txt");
        }

        public async Task<string> GetWeatherAsync()
        {
            if (File.Exists(_cacheFile))
            {
                var info = new FileInfo(_cacheFile);
                // Проверяем срок жизни файла (1 минута)
                if (DateTime.Now - info.LastWriteTime < TimeSpan.FromMinutes(1))
                    return await File.ReadAllTextAsync(_cacheFile);
            }

            // Эмуляция запроса к данным
            var weather = "Cloudy";
            await File.WriteAllTextAsync(_cacheFile, weather);
            return weather;
        }
    }
}
