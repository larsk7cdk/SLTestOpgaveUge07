using System.Globalization;
using FluentAssertions;
using Moq;
using Vejrudsigten.Services;

namespace Vejrudsigten.Test;

public class WeatherServiceTest
{
    [Fact]
    public async Task Noegle_som_indeholder_et_korrekt_antal_karakterer_er_gyldig()
    {
        // Arrange
        const string key = "1";
        const string location = "Kolding";

        var weatherServiceTestDouble = CreateWeatherServiceWithParameters();
        var sut = new WeatherForecast(weatherServiceTestDouble.Object);

        // Act
        var actual = await Record.ExceptionAsync(() => sut.GetForecastAsync(key, location));

        // Assert
        actual.Should().BeNull();
    }

    [Fact]
    public async Task Noegle_som_indeholder_et_ukorrekt_antal_karakterer_er_ugyldig()
    {
        // Arrange
        const string key = "";
        const string location = "Kolding";

        var weatherServiceTestDouble = CreateWeatherServiceWithParameters();
        var sut = new WeatherForecast(weatherServiceTestDouble.Object);

        // Act
        var actual = await Record.ExceptionAsync(() => sut.GetForecastAsync(key, location));

        // Assert
        actual.Should().BeOfType<ArgumentException>();
    }

    [Theory]
    [InlineData("12")]
    [InlineData("01234567890123456789012345678901234567890123456789")]
    public async Task By_som_indeholder_et_korrekt_antal_karakterer_er_gyldig(string location)
    {
        // Arrange
        const string key = "key";

        var weatherServiceTestDouble = CreateWeatherServiceWithParameters();
        var sut = new WeatherForecast(weatherServiceTestDouble.Object);

        // Act
        var actual = await Record.ExceptionAsync(() => sut.GetForecastAsync(key, location));

        // Assert
        actual.Should().NotBeOfType<ArgumentException>();
    }

    [Theory]
    [InlineData("1")]
    [InlineData("012345678901234567890123456789012345678901234567890")]
    public async Task By_som_indeholder_et_ukorrekt_antal_karakterer_er_ugyldig(string location)
    {
        // Arrange
        const string key = "key";

        var weatherServiceTestDouble = CreateWeatherServiceWithParameters();
        var sut = new WeatherForecast(weatherServiceTestDouble.Object);

        // Act
        var actual = await Record.ExceptionAsync(() => sut.GetForecastAsync(key, location));

        // Assert
        actual.Should().BeOfType<ArgumentException>();
    }

    [Fact]
    public async Task Vejrmelding_som_indeholder_by_og_vejrtype_og_temperatur_for_idag_og_dagen_foer_er_gyldig()
    {
        // Arrange 
        const string key = "key";
        const string location = "Kolding";
        const string conditionsToday = "Klart vejr";
        const double tempToday = 20.7;
        const string conditionsYesterday = "Sne";
        const double tempYesterday = -5.2;

        var weatherServiceTestDouble = CreateWeatherServiceWithParameters(location, conditionsToday, tempToday, conditionsYesterday, tempYesterday);
        var sut = new WeatherForecast(weatherServiceTestDouble.Object);

        // Act
        var actual = await sut.GetForecastAsync(key, location);

        // Assert
        actual.Forecast.Should().Contain(location);
        actual.Forecast.Should().Contain(conditionsToday);
        actual.Forecast.Should().Contain(tempToday.ToString(CultureInfo.CurrentCulture));
        actual.Forecast.Should().Contain(conditionsYesterday);
        actual.Forecast.Should().Contain(tempYesterday.ToString(CultureInfo.CurrentCulture));
    }

    [Theory]
    [InlineData("Kolding", "Klart vejr", 5, "Skyet", 7, "Pas på når i cykler, det kan blive glat")]
    [InlineData("Kolding", "Klart vejr", 15, "Skyet", 7, "Det indbyder til en gåtur")]
    [InlineData("Kolding", "Klart vejr", 25, "Skyet", 7, "Så det afsted til stranden")]
    [InlineData("Kolding", "Regn", 15, "Skyet", 7, "Paraplyen skal i brug")]
    [InlineData("Kolding", "Sne", 5, "Skyet", 7, "Husk vanterne til de små poder, det bliver koldt")]
    [InlineData("Kolding", "Skyet", 15, "Skyet", 7, "Foråret er på vej")]
    [InlineData("Kolding", "Skyet", 25, "Skyet", 7, "Solcellerne har ikke de bedste vilkår")]
    public async Task Vejrmelding_som_indeholder_by_og_vejrtype_og_temperatur_for_idag_og_dagen_foer_viser_en_overskrift(
        string location,
        string conditionsToday,
        double tempToday,
        string conditionsYesterday,
        double tempYesterday,
        string headline)
    {
        // Arrange 
        const string key = "key";

        var weatherServiceTestDouble = CreateWeatherServiceWithParameters(location, conditionsToday, tempToday, conditionsYesterday, tempYesterday);
        var sut = new WeatherForecast(weatherServiceTestDouble.Object);

        // Act
        var actual = await sut.GetForecastAsync(key, location);

        // Assert
        actual.Headline.Should().Be(headline);
    }


    #region Helpers

    private Mock<IWeatherService> CreateWeatherServiceWithParameters(
        string location = "Kolding",
        string conditionsToday = "Andet", double tempToday = 0.0,
        string conditionsYesterday = "Andet", double tempYesterday = 0.0)
    {
        var weatherServiceTestDouble = new Mock<IWeatherService>();

        var weatherInfoToday = new WeatherInfo
        {
            Conditions = conditionsToday,
            Temperature = tempToday
        };

        var weatherInfoYesterday = new WeatherInfo
        {
            Conditions = conditionsYesterday,
            Temperature = tempYesterday
        };

        weatherServiceTestDouble.Setup(s => s.GetTodaysWeather(It.IsAny<string>(), location)).ReturnsAsync(weatherInfoToday);
        weatherServiceTestDouble.Setup(s => s.GetYesterdaysWeather(It.IsAny<string>(), location)).ReturnsAsync(weatherInfoYesterday);

        return weatherServiceTestDouble;
    }

    #endregion
}