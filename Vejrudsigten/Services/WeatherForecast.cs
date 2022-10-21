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

    public async Task<WeatherForecastInfo> GetForecastAsync(string key, string location)
    {
        ValidateForeCast(key, location);

        var todayInfo = await _service.GetTodaysWeather(key, location);
        var yesterdayInfo = await _service.GetYesterdaysWeather(key, location);

        const string forecastText = "Vejret i {0} er {1} og der er {2} grader. I går var det {3} og {4} grader";
        var forecast = string.Format(forecastText, location, todayInfo.Conditions, todayInfo.Temperature, yesterdayInfo.Conditions, yesterdayInfo.Temperature);
        var headline = GetHeadline(todayInfo);

        return new WeatherForecastInfo
        {
            Forecast = forecast,
            Headline = headline
        };
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

    private static string GetHeadline(WeatherInfo weatherInfo)
    {
        return (weatherInfo.Conditions, weatherInfo.Temperature) switch
        {
            ("Klart vejr", < 10) => "Pas på når i cykler, det kan blive glat",
            ("Klart vejr", >= 10 and <= 20) => "Det indbyder til en gåtur",
            ("Klart vejr", > 20) => "Så det afsted til stranden",
            ("Regn", >= 10 and <= 20) => "Paraplyen skal i brug",
            ("Sne", < 10) => "Husk vanterne til de små poder, det bliver koldt",
            ("Skyet", >= 10 and <= 20) => "Foråret er på vej",
            ("Skyet", > 20) => "Solcellerne har ikke de bedste vilkår",
            _ => ""
        };
    }
}