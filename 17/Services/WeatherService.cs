using Microsoft.Extensions.Caching.Memory;
using System;

namespace MyCachingApp.Services
{
    public class WeatherService
    {
        private readonly IMemoryCache _cache;

        public WeatherService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public string GetWeather()
        {
            return _cache.GetOrCreate("weather", entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30);
                return "Sunny"; // Эмуляция данных
            });
        }
    }
}
