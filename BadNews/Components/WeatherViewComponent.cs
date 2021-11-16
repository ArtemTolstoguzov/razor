using System;
using System.Threading.Tasks;
using BadNews.Repositories.News;
using BadNews.Repositories.Weather;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace BadNews.Components
{
    public class WeatherViewComponent : ViewComponent
    {
        private readonly IWeatherForecastRepository weatherRepository;
        private readonly OpenWeatherOptions weatherOptions;

            public WeatherViewComponent(IWeatherForecastRepository weatherRepository, IOptions<OpenWeatherOptions> weatherOptions)
            {
                this.weatherRepository = weatherRepository;
                this.weatherOptions = weatherOptions.Value;
            }

            public async Task<IViewComponentResult> InvokeAsync()
            {
                var apiKey = weatherOptions.ApiKey;
                var weatherClient = new OpenWeatherClient(apiKey);
                WeatherForecast weather;
                try
                {
                    var weatherFromClient = await weatherClient.GetWeatherFromApiAsync();
                    weather = WeatherForecast.CreateFrom(weatherFromClient);
                }
                catch (Exception e)
                {
                    weather = await weatherRepository.GetWeatherForecastAsync();
                }
                return View(weather);
            }
    }
}