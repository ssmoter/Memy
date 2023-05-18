using Memy.Server.Data;
using Memy.Server.Data.SqlDataAccess;
using Memy.Server.Data.User;
using Memy.Server.Filtres;
using Memy.Shared;

using Microsoft.AspNetCore.Mvc;

namespace Memy.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly ISqlDataAccess _sqlDataAccess;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, ISqlDataAccess sqlDataAccess)
        {
            _logger = logger;
            _sqlDataAccess = sqlDataAccess;
        }

        [TokenAuthenticationFilter]
        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}