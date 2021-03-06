using System;

namespace IntegrationTestingExample.IntegrationTests.Configuration
{
    public class TokenCreatorConfiguration
    {
        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string Scopes { get; set; }

        public Guid TenantId { get; set; }

        public string TestUsername { get; set; }

        public string TestPassword { get; set; }
    }
}
