using System;
using System.Threading.Tasks;

namespace Vejrudsigten.Services;

public class WeatherForecast : IWeatherForecast
{
    private readonly IWeatherService _service;

    public WeatherForecast(IWeatherService service)
    {
        _service = service;
    }

    public async Task<string> GetForecastAsync(string key, string location)
    {
        ValidateForeCast(key, location);

        var todayInfo = await _service.GetTodaysWeather(key, location);
        var yesterdayInfo = await _service.GetYesterdaysWeather(key, location);

        var result = "Vejret i {0} er {1} og der er {2} grader. I går var det {3} og {4} grader";
        return string.Format(result, location, todayInfo.Conditions, todayInfo.Temperature, yesterdayInfo.Conditions, yesterdayInfo.Temperature);
    }

    public async Task<string> GetForecastAsync(string key, string location, string weatherMessage)
    {
        ValidateForeCast(key, location);

        var todayInfo = await _service.GetTodaysWeather(key, location);
        var yesterdayInfo = await _service.GetYesterdaysWeather(key, location);

        return string.Format(weatherMessage, location, todayInfo.Conditions, todayInfo.Temperature, yesterdayInfo.Conditions, yesterdayInfo.Temperature);
    }

    private static void ValidateForeCast(string key, string location)
    {
        if (string.IsNullOrEmpty(key))
            throw new ArgumentException("Key is missing");

        if (string.IsNullOrEmpty(location))
            throw new ArgumentException("Location is missing");

        if (location.Length is < 2 or > 50)
            throw new ArgumentException("Location should be between 2 and 50 characters");
    }
}