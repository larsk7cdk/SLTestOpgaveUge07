using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Vejrudsigten.Services;

namespace Vejrudsigten.Pages;

public class IndexModel : PageModel
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<IndexModel> _logger;

    public IndexModel(ILogger<IndexModel> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    public async Task OnGetAsync()
    {
        var key = _configuration["key"];

        if (key == null)
        {
            ViewData.Add("Vejrudsigten",
                "Hov! Du har glemt at angive nøglen i appsettings.local.json. Gå tilbage til opgavebeskrivelsen og se hvordan");
        }
        else
        {
            var weatherService = new WeatherService();
            ViewData.Add("Vejrudsigten", await new WeatherForecast(weatherService).GetForecastAsync(key, "Kolding"));
        }
    }
}