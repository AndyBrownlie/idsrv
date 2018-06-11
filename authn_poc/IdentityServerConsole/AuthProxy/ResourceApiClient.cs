using System.Collections.Generic;
using IdentityServer3.Core.Models;

namespace AuthProxy
{
    public class ResourceApiClient : Client
    {
        public ResourceApiClient(string clientName, string clientId, string clientSecret, params string[] allowedScopes) : this()
        {

            ClientName = clientName; 
            ClientId = clientId;
            ClientSecrets = new List<Secret> { new Secret(clientSecret.Sha256()) };
            AllowedScopes = new List<string>(allowedScopes);
        }

        private ResourceApiClient()
        {
            Enabled = true;
            Flow = Flows.Hybrid;
            RequireConsent = false;
        }
    }
}