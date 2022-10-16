using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vejrudsigten.Services
{
    public class WeatherInfo
    {
        /// <summary>
        /// En af følgende vejrtyper:
        /// Klart vejr
        /// Regn
        /// Sne
        /// Skyet
        /// Andet
        /// </summary>
        public string Conditions { get; set; }

        /// <summary>
        /// Temperaturen i celcius
        /// </summary>
        public double Temperature { get; set; }
    }
}
