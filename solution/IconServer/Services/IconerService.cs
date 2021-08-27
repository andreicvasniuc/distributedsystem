using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace IconServer
{
    public class IconerService : Iconer.IconerBase
    {
        private readonly ILogger<IconerService> _logger;
        private readonly IDictionary<string, string> _icons;
        public IconerService(ILogger<IconerService> logger)
        {
            _logger = logger;
            _icons = new Dictionary<string, string> {
                { "Freezing", "https://cdn2.iconfinder.com/data/icons/winter-travel-indigo-vol-1/256/Freezing-128.png" },
                { "Bracing", "https://cdn3.iconfinder.com/data/icons/weather-free-1/32/Weather_Free_Filled_Outline_freezing-termometer-weather-cold-temperature-128.png" },
                { "Chilly", "https://cdn1.iconfinder.com/data/icons/weather-377/24/weather_forcast_freezing_temperature_snow_winter-128.png" },
                { "Cool", "https://cdn4.iconfinder.com/data/icons/the-weather-is-nice-today/64/weather_42-128.png" },
                { "Mild", "https://cdn4.iconfinder.com/data/icons/the-weather-is-nice-today/64/weather_43-128.png" },
                { "Warm", "https://cdn3.iconfinder.com/data/icons/bebreezee-weather-symbols/690/icon-weather-suncloudlight-128.png" },
                { "Balmy", "https://cdn3.iconfinder.com/data/icons/bebreezee-weather-symbols/559/icon-weather-sunny-128.png" },
                { "Hot", "https://cdn0.iconfinder.com/data/icons/summer-253/512/sun_summer_sunny_weather_summertime_warm_holidays_meteorology_nature-128.png" },
                { "Sweltering", "https://cdn3.iconfinder.com/data/icons/nature-emoji/50/Hot-128.png" },
                { "Scorching", "https://cdn1.iconfinder.com/data/icons/climate-change-11/64/Temperature-hot-warm-weather-forecast-thermometer-mercury-climate-128.png" }
            };
        }

        public override Task<IconReply> GetIcon(IconRequest request, ServerCallContext context)
        {
            return Task.FromResult(new IconReply
            {
                IconUrl = _icons[request.WeatherSummary]
            });
        }
    }
}
