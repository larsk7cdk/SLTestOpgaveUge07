using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Vejrudsigten.Services
{
    public class WeatherService
    {
        public async Task<WeatherInfo> GetTodaysWeather(string key, string location)
        {
            HttpClient client = new HttpClient();
            String urlPattern = "https://smartweatherdk.azurewebsites.net/api/GetTodaysWeather?key={0}&location={1}";            
            var url = String.Format(urlPattern, key, location);
            var streamTask = client.GetStreamAsync(url);
            var weatherInfo = await JsonSerializer.DeserializeAsync<WeatherInfo>(await streamTask);
            return weatherInfo;
        }

        public async Task<WeatherInfo> GetYesterdaysWeather(string key, string location)
        {
            HttpClient client = new HttpClient();
            String urlPattern = "https://smartweatherdk.azurewebsites.net/api/GetYesterdaysWeather?key={0}&location={1}";
            var url = String.Format(urlPattern, key, location);
            var streamTask = client.GetStreamAsync(url);
            var weatherInfo = await JsonSerializer.DeserializeAsync<WeatherInfo>(await streamTask);
            return weatherInfo;
        }

    }
}
