using Microsoft.AspNetCore.Mvc;
using MyCachingApp.Services;
using System.Threading.Tasks;

namespace MyCachingApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {
        private readonly WeatherService _memoryCache;
        private readonly FileCacheService _fileCache;

        public WeatherController(WeatherService memoryCache, FileCacheService fileCache)
        {
            _memoryCache = memoryCache;
            _fileCache = fileCache;
        }

        [HttpGet("memory")]
        public IActionResult GetMemoryCache()
        {
            return Ok(_memoryCache.GetWeather());
        }

        [HttpGet("file")]
        public async Task<IActionResult> GetFileCache()
        {
            return Ok(await _fileCache.GetWeatherAsync());
        }

        [ResponseCache(Duration = 60)]
        [HttpGet("response")]
        public IActionResult GetResponseCache()
        {
            return Ok("Response cached at " + System.DateTime.Now.ToLongTimeString());
        }
    }
}
