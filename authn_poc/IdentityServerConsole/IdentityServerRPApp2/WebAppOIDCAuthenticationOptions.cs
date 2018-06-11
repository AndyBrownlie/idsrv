using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Owin.Security.OpenIdConnect;
using ConfigManager = System.Configuration.ConfigurationManager;

namespace IdentityServerWebAPI
{
    public class WebAppOIDCAuthenticationOptions: OpenIdConnectAuthenticationOptions
    {
        public WebAppOIDCAuthenticationOptions(string clientOidcConfigPrefix) : this()
        {
            ClientId = ConfigManager.AppSettings[$"{clientOidcConfigPrefix}:oidc:ClientId"];
            RedirectUri = ConfigManager.AppSettings[$"{clientOidcConfigPrefix}:oidc:RedirectUri"];
        }

        private WebAppOIDCAuthenticationOptions()
        {
            Authority = ConfigManager.AppSettings["oidc:Authority"];
            Scope = ConfigManager.AppSettings["oidc:Scope"];
            ResponseType = ConfigManager.AppSettings["oidc:ResponseType"];
            SignInAsAuthenticationType = ConfigManager.AppSettings["oidc:AuthenticationType"];
            UseTokenLifetime = false;
            SetNotifications();
        }

        private void SetNotifications()
        {
            Notifications = new OpenIdConnectAuthenticationNotifications()
            {
                RedirectToIdentityProvider = (context) =>
                {
                    Debug.WriteLine("*** RedirectToIdentityProvider");
                    return Task.FromResult(0);
                },
                MessageReceived = (context) =>
                {
                    Debug.WriteLine("*** MessageReceived");
                    return Task.FromResult(0);
                },
                SecurityTokenReceived = (context) =>
                {
                    Debug.WriteLine("*** SecurityTokenReceived");
                    return Task.FromResult(0);
                },
                SecurityTokenValidated = (context) =>
                {
                    Debug.WriteLine("*** SecurityTokenValidated");
                    var ticket = context.AuthenticationTicket;
                    return Task.FromResult(0);
                },
                AuthorizationCodeReceived = (context) =>
                {
                    Debug.WriteLine("*** AuthorizationCodeReceived");
                    return Task.FromResult(0);
                },
                AuthenticationFailed = (context) =>
                {
                    Debug.WriteLine("*** AuthenticationFailed");
                    return Task.FromResult(0);
                },
            };

        }
    }
}