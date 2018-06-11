using System.Threading.Tasks;
using IdentityServer3.Core.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OpenIdConnect;
using ConfigManager = System.Configuration.ConfigurationManager;

namespace AuthProxy
{
    public class OidcExternalIdentityProvider: OpenIdConnectAuthenticationOptions
    {
        public OidcExternalIdentityProvider(string identityProviderConfigPrefix) : base (identityProviderConfigPrefix)
        {
            AuthenticationType = ConfigManager.AppSettings[$"{identityProviderConfigPrefix}:AuthType"]; 
            Caption = ConfigManager.AppSettings[$"{ identityProviderConfigPrefix}:Caption"]; 
            ClientId = ConfigManager.AppSettings[$"{identityProviderConfigPrefix}:ClientId"];
            ClientSecret = ConfigManager.AppSettings[$"{identityProviderConfigPrefix}:ClientSecret"].Sha256();
            Authority = ConfigManager.AppSettings[$"{identityProviderConfigPrefix}:Authority"];
            GenericConfiguration();
        }

        private void GenericConfiguration() 
        {
            RedirectUri = ConfigManager.AppSettings["IdSvr:RedirectUri"];
            ResponseType = ConfigManager.AppSettings["IdSvr:ResponseType"];
            Scope = ConfigManager.AppSettings["IdSvr:Scope"];
            AuthenticationMode = AuthenticationMode.Active;
             
            TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                AuthenticationType = IdentityServer3.Core.Constants.ExternalAuthenticationType
            };
            UseTokenLifetime = false;
            //Notifications = DefineAuthenticationOptions([$"{identityProviderConfigPrefix}:RedirectUri"])  
        }

        // useful if we want to intercept callback and populate with AuthZ claims
        private static OpenIdConnectAuthenticationNotifications DefineAuthenticationOptions(string redirectUri)
        {
            return new OpenIdConnectAuthenticationNotifications
            {
                SecurityTokenValidated = n =>
                {
                    n.AuthenticationTicket.Properties.RedirectUri = redirectUri;
                    return Task.CompletedTask;
                }
            };
        }
    }
}