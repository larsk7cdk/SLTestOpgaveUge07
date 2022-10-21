using System.Threading.Tasks;

namespace Vejrudsigten.Services;

public interface IWeatherForecast
{
    Task<WeatherForecastInfo> GetForecastAsync(string key, string location);
}