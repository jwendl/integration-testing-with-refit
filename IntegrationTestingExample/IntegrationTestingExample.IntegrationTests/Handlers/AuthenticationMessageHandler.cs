using IntegrationTestingExample.IntegrationTests.Authentication;
using IntegrationTestingExample.IntegrationTests.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace IntegrationTestingExample.IntegrationTests.Handlers
{
    public class AuthenticationMessageHandler
        : DelegatingHandler
    {
        private readonly ITokenCreator tokenCreator;

        public AuthenticationMessageHandler(ITokenCreator tokenCreator, IOptions<TokenCreatorConfiguration> options)
        {
            this.tokenCreator = tokenCreator ?? throw new ArgumentNullException(nameof(tokenCreator));
            _ = options ?? throw new ArgumentNullException(nameof(tokenCreator));
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            _ = request ?? throw new ArgumentNullException(nameof(request));

            var httpRequestHeaders = request.Headers;

            // If you have the following attribute in your interface, the authorization header will be "Bearer", not null.
            // [Headers("Authorization: Bearer")]
            // If we have a token, then we want to use that token - otherwise generate a service to service one.
            var authenticationHeaderValue = httpRequestHeaders.Authorization;
            if (authenticationHeaderValue != null)
            {
                var accessToken = await tokenCreator.GetIntegrationTestTokenAsync();
                httpRequestHeaders.Authorization = new AuthenticationHeaderValue(authenticationHeaderValue.Scheme, accessToken);
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
