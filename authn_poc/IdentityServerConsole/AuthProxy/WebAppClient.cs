using System.Collections.Generic;
using IdentityServer3.Core.Models;

namespace AuthProxy
{
    public class WebAppClient : Client
    {
        public WebAppClient(string clientName, string clientId, string clientSecret, params string[] redirectUris) : this()
        {

            ClientName = clientName; 
            ClientId = clientId;
            ClientSecrets = new List<Secret> { new Secret(clientSecret.Sha256()) };
            RedirectUris = new List<string>(redirectUris);
            AllowedScopes = new List<string>
            {
                "openid",
                "profile",
                "roles",
                "sampleApi"
            };
        }

        private WebAppClient()
        {
            Enabled = true;
            Flow = Flows.Hybrid;
            RequireConsent = false;
        }
    }
}