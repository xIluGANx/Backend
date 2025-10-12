using Microsoft.Extensions.Caching.Distributed;
using System.Threading.Tasks;

namespace MyCachingApp.Services
{
    public class DistributedCacheService
    {
        private readonly IDistributedCache _cache;

        public DistributedCacheService(IDistributedCache cache)
        {
            _cache = cache;
        }

        public async Task<string> GetWeatherAsync()
        {
            var cachedData = await _cache.GetStringAsync("weather");
            if (!string.IsNullOrEmpty(cachedData))
                return cachedData;

            var weather = "Cloudy"; // Эмуляция данных
            await _cache.SetStringAsync("weather", weather, new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1)
            });
            return weather;
        }
    }
}
