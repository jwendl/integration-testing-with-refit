using IntegrationTestingExample.IntegrationTests.Authentication;
using IntegrationTestingExample.IntegrationTests.Configuration;
using IntegrationTestingExample.IntegrationTests.Handlers;
using IntegrationTestingExample.IntegrationTests.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using Polly;
using Polly.Extensions.Http;
using Refit;
using System;

namespace IntegrationTestingExample.IntegrationTests.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddRefitDependencies(this ServiceCollection serviceCollection)
        {
            var configuration = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddJsonFile("appsettings.json")
                .Build();

            var clientConfiguration = new ClientConfiguration();
            configuration.Bind(nameof(ClientConfiguration), clientConfiguration);
            serviceCollection.Configure<ClientConfiguration>(cc => configuration.Bind(nameof(ClientConfiguration), cc));

            serviceCollection.Configure<TokenCreatorConfiguration>(tcc => configuration.Bind(nameof(TokenCreatorConfiguration), tcc));
            serviceCollection.AddSingleton<ITokenCreator, TokenCreator>();
            serviceCollection.AddSingleton<AbstractApplicationBuilder<PublicClientApplicationBuilder>, PublicClientApplicationBuilder>((sp) =>
            {
                var options = sp.GetRequiredService<IOptions<TokenCreatorConfiguration>>();
                var tokenCreatorConfiguration = options.Value;

                return PublicClientApplicationBuilder
                    .Create(tokenCreatorConfiguration.ClientId)
                    .WithAuthority(AzureCloudInstance.AzurePublic, tokenCreatorConfiguration.TenantId);
            });

            var asyncRetryPolicy = HttpPolicyExtensions.HandleTransientHttpError()
                .WaitAndRetryAsync(3, retryAttempts => TimeSpan.FromMilliseconds(Math.Pow(2, retryAttempts)));

            serviceCollection.AddSingleton<AuthenticationMessageHandler>();

            serviceCollection.AddRefitClient<IIntegrationTestClient>()
                .AddPolicyHandler(asyncRetryPolicy)
                .AddHttpMessageHandler<AuthenticationMessageHandler>()
                .ConfigureHttpClient(http => http.BaseAddress = clientConfiguration.ApiServiceUri);
        }
    }
}
