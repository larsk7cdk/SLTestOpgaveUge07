using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Vejrudsigten.Services;

public class WeatherService : IWeatherService
{
    public async Task<WeatherInfo> GetTodaysWeather(string key, string location)
    {
        var client = new HttpClient();
        var urlPattern = "https://smartweatherdk.azurewebsites.net/api/GetTodaysWeather?key={0}&location={1}";
        var url = string.Format(urlPattern, key, location);
        var streamTask = client.GetStreamAsync(url);
        var weatherInfo = await JsonSerializer.DeserializeAsync<WeatherInfo>(await streamTask);
        return weatherInfo;
    }

    public async Task<WeatherInfo> GetYesterdaysWeather(string key, string location)
    {
        var client = new HttpClient();
        var urlPattern = "https://smartweatherdk.azurewebsites.net/api/GetYesterdaysWeather?key={0}&location={1}";
        var url = string.Format(urlPattern, key, location);
        var streamTask = client.GetStreamAsync(url);
        var weatherInfo = await JsonSerializer.DeserializeAsync<WeatherInfo>(await streamTask);
        return weatherInfo;
    }
}