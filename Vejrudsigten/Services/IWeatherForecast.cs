using System.Threading.Tasks;

namespace Vejrudsigten.Services;

public interface IWeatherForecast
{
    Task<string> GetForecastAsync(string key, string location);
    Task<string> GetForecastAsync(string key, string location, string weatherMessage);
}