using System.Collections.Generic;
using IdentityServer3.Core.Models;
using ConfigManager = System.Configuration.ConfigurationManager;

namespace AuthProxy
{
    public static class ClientsFactory
    {
        public static IEnumerable<Client> Build()
        {
            return new Client[]
            {
                new WebAppClient("MVC Client 1", ConfigManager.AppSettings["App1:ClientId"], ConfigManager.AppSettings["App1:ClientSecret"], ConfigManager.AppSettings["App1:RedirectUri"]),
                new WebAppClient("MVC Client 2", ConfigManager.AppSettings["App2:ClientId"], ConfigManager.AppSettings["App2:ClientSecret"], ConfigManager.AppSettings["App2:RedirectUri"]),
                new ResourceApiClient("Resource API 1", ConfigManager.AppSettings["Api1:ClientId"], ConfigManager.AppSettings["Api1:ClientSecret"], ConfigManager.AppSettings["Api1:AllowedScopes"])
            };
        }
    }
}