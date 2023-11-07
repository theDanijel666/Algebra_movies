using Microsoft.AspNetCore.Mvc;

namespace Movies.API.Controllers
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

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("{date}")]
        public ActionResult<WeatherForecast> GetWeatherForecastByDate(DateOnly date)
        {
            var weatherForecast = Get().FirstOrDefault(wf => wf.Date == date);

            if (weatherForecast == null)
            {
                return NotFound();
            }

            return weatherForecast;
        }

        [HttpPost]
        public string HelloWorld()
        {
            return "Hello World";
        }

        [HttpGet("{int}")]
        public string HelloWorld2(int a)
        {
            return "Hello World";
        }

        private string GetHelloWorld()
        {
            return "Hello World";
        }
    }
}