using IntegrationTestingExample.IntegrationTests.Models;
using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IntegrationTestingExample.IntegrationTests.Interfaces
{
    public interface IIntegrationTestClient
    {
        [Headers("Authorization: Bearer")]
        [Get("/WeatherForecast")]
        Task<IEnumerable<WeatherForecast>> FetchWeatherForcastsAsync();
    }
}
