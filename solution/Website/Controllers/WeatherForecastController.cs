using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EasyNetQ;
using Messages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Website.Controllers
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
        private readonly IBus _bus;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IBus bus)
        {
            _logger = logger;
            _bus = bus;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            var weatherForecastList = Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();

            NotifyAboutWeatherForecast(weatherForecastList);

            return weatherForecastList;
        }

        private void NotifyAboutWeatherForecast(WeatherForecast[] weatherForecastList)
        {
            var messages = weatherForecastList.Select(weatherForecast => new WeatherForecastMessage {
                WeatherForecastDate = weatherForecast.Date,
                WeatherTemperatureC = weatherForecast.TemperatureC,
                WeatherSummary = weatherForecast.Summary,
                MessageSentDate = DateTimeOffset.UtcNow
            });

            messages.ToList().ForEach(message => _bus.PubSub.Publish<WeatherForecastMessage>(message));
        }
    }
}
