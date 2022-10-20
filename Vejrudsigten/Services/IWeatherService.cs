using System.Threading.Tasks;

namespace Vejrudsigten.Services;

public interface IWeatherService
{
    Task<WeatherInfo> GetTodaysWeather(string key, string location);
    Task<WeatherInfo> GetYesterdaysWeather(string key, string location);
}