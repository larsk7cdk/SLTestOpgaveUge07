using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Vejrudsigten.Services;

namespace Vejrudsigten.IntegrationsTest;

public class WeatherServiceTest
{
    private readonly IConfiguration _configuration = new ConfigurationBuilder().AddUserSecrets<WeatherServiceTest>().Build();

    [Fact]
    public async Task Vejret_i_hvidovre_kan_hentes()
    {
        // Arrange
        const string location = "Hvidovre";

        var key = _configuration["key"];
        var weatherService = new WeatherService();
        var sut = new WeatherForecast(weatherService);

        // Act
        var actual = await sut.GetForecastAsync(key, location);

        // Assert
        actual.Forecast.Should().Contain(location);
    }
}