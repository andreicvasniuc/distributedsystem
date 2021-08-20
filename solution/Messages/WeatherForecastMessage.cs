using System;

namespace Messages
{
    public class WeatherForecastMessage
    {
        public DateTime WeatherForecastDate { get; set; }
        public int WeatherTemperatureC { get; set; }
        public string WeatherSummary { get; set; }
        public DateTimeOffset MessageSentDate { get; set; }
    }
}
