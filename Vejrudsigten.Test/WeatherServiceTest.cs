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
        actual.Should().Contain(location);
        actual.Should().Contain(conditionsToday);
        actual.Should().Contain(tempToday.ToString(CultureInfo.CurrentCulture));
        actual.Should().Contain(conditionsYesterday);
        actual.Should().Contain(tempYesterday.ToString(CultureInfo.CurrentCulture));
    }


    [Theory]
    [InlineData(@"Vejret i {0} er {1} og der er {2} grader. I går var det {3} og {4} grader, så det er bare om at komme til stranden", "Kolding","Klart vejr", 25.3, "Skyet", 19.8)]
    [InlineData(@"Vejret i {0} er {1} med {2} grader. I går {3} og {4} grader, så folk med solceller er glade", "Hvidovre", "Klart vejr", 20.4, "Klart vejr", 23.1)]
    public async Task Anvender_angiver_vejrmelding_som_indeholder_by_og_vejrtype_og_temperatur_for_idag_og_dagen_foer_er_gyldig(
        string message,
        string location,
        string conditionsToday,
        double tempToday,
        string conditionsYesterday,
        double tempYesterday)
    {
        // Arrange 
        const string key = "key";
        var expected = string.Format(message, location, conditionsToday, tempToday.ToString(CultureInfo.CurrentCulture), conditionsYesterday, tempYesterday.ToString(CultureInfo.CurrentCulture));

        var weatherServiceTestDouble = CreateWeatherServiceWithParameters(location, conditionsToday, tempToday, conditionsYesterday, tempYesterday);
        var sut = new WeatherForecast(weatherServiceTestDouble.Object);

        // Act
        var actual = await sut.GetForecastAsync(key, location, message);

        // Assert
        actual.Should().Be(expected);
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