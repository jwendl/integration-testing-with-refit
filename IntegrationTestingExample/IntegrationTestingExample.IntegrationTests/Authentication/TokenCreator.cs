using IntegrationTestingExample.IntegrationTests.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using System;
using System.Security;
using System.Threading.Tasks;

namespace IntegrationTestingExample.IntegrationTests.Authentication
{
    public interface ITokenCreator
    {
        Task<string> GetIntegrationTestTokenAsync();
    }

    public class TokenCreator
        : ITokenCreator
    {
        private readonly TokenCreatorConfiguration _tokenCreatorConfiguration;
        private readonly AbstractApplicationBuilder<PublicClientApplicationBuilder> _publicClientApplicationBuilder;
        private readonly ILogger<TokenCreator> _logger;

        public TokenCreator(IOptions<TokenCreatorConfiguration> options, AbstractApplicationBuilder<PublicClientApplicationBuilder> publicClientApplicationBuilder, ILogger<TokenCreator> logger)
        {
            _ = options ?? throw new ArgumentNullException(nameof(options));
            _publicClientApplicationBuilder = publicClientApplicationBuilder ?? throw new ArgumentNullException(nameof(publicClientApplicationBuilder));

            _tokenCreatorConfiguration = options.Value;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<string> GetIntegrationTestTokenAsync()
        {
            using var securePassword = new SecureString();
            foreach (var c in _tokenCreatorConfiguration.TestPassword)
            {
                securePassword.AppendChar(c);
            }

            try
            {
                var publicClientApplication = _publicClientApplicationBuilder
                    .WithTenantId(_tokenCreatorConfiguration.TenantId.ToString())
                    .Build();

                var scopes = _tokenCreatorConfiguration.Scopes.Split(' ');
                var authenticationResult = await publicClientApplication
                    .AcquireTokenByUsernamePassword(scopes, _tokenCreatorConfiguration.TestUsername, securePassword)
                    .ExecuteAsync();
                return authenticationResult.AccessToken;
            }
            catch (MsalServiceException msalServiceException)
            {
                _logger.LogError(msalServiceException, msalServiceException.Message);
                throw new InvalidOperationException("There was an issue with MSAL library call", msalServiceException);
            }
        }
    }
}
