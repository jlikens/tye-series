using System;
using System.Collections.Generic;
using System.Text;

namespace frontend.Shared
{
    public class WeatherForecastResource
    {
        public string PostalCode { get; set; }

        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF { get; set; }

        public string Summary { get; set; }
    }
}
