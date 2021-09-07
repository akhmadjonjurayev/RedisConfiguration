using ConfigureRedis.Extension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfigureRedis.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        private readonly IDistributedCache _cache;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IDistributedCache cache)
        {
            _cache = cache;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
        [HttpPost]
        public IActionResult SetDataToRedis([FromBody]KeyData data)
        {
            if(data.Key == default)
            {
                return BadRequest(new { message = "error-invalid-data" });
            }
            else
            {
                _cache.SetData<object>(data.Key, new { name = "akhmadjon", lavel = "senior arxitektor" });
                return Ok(new { message = "everything will be ok" });
            }
        }
        [HttpGet("getRedis")]
        public IActionResult GetDataFromRedis([FromQuery] string key)
        {
            //var data = _cache.GetData<object>(key);
            //if (data == default)
            //    return BadRequest(new { message = "error-not-found-data" });
            //return Ok(data);
            //var data = _cache.Get(key);
            //return Ok(Encoding.UTF8.GetString(data));
            try
            {
                return Ok(_cache.GetData<List<Staff>>(key));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("another")]
        public IActionResult SetDataAnotherWay([FromBody]KeyData data)
        {
            try
            {
                _cache.Set("eldor", Encoding.UTF8.GetBytes(data.Key));
                return Ok("success-redis-add-data");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("staff")]
        public IActionResult SetStaffData([FromBody]List<Staff> staffs,string key)
        {
            try
            {
                _cache.SetData<List<Staff>>(key, staffs);
                return Ok("success-load-data");
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
