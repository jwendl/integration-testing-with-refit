using FluentAssertions;
using IntegrationTestingExample.IntegrationTests.Extensions;
using IntegrationTestingExample.IntegrationTests.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace IntegrationTestingExample.IntegrationTests.Controllers
{
    public class WeatherForecastControllerTests
    {
        private readonly IServiceProvider _serviceProvider;

        public WeatherForecastControllerTests()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddRefitDependencies();

            _serviceProvider = serviceCollection.BuildServiceProvider();
        }

        [Fact]
        public async Task WeatherForecastController_CanCallGet_GetResults()
        {
            var integrationTestClient = _serviceProvider.GetRequiredService<IIntegrationTestClient>();
            var weatherForcasts = await integrationTestClient.FetchWeatherForcastsAsync();
            weatherForcasts.Count().Should().Be(5);
        }
    }
}
