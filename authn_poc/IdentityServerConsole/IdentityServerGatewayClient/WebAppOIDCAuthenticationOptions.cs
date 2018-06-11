using System.Threading.Tasks;
using log4net;
using Microsoft.Owin.Security.OpenIdConnect;
using ConfigManager = System.Configuration.ConfigurationManager;

namespace OIDCGatewayClient
{
    public class WebAppOidcAuthenticationOptions: OpenIdConnectAuthenticationOptions
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public WebAppOidcAuthenticationOptions()
        {
            ClientId = ConfigManager.AppSettings["oidc:ClientId"];
            ClientSecret = ConfigManager.AppSettings["oidc:ClientSecret"];
            RedirectUri = ConfigManager.AppSettings["oidc:RedirectUri"];
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
                    Logger.Debug("*** RedirectToIdentityProvider");
                    return Task.FromResult(0);
                },
                MessageReceived = (context) =>
                {
                    Logger.Debug("*** MessageReceived");
                    return Task.FromResult(0);
                },
                SecurityTokenReceived = (context) =>
                {
                    Logger.Debug("*** SecurityTokenReceived");
                    return Task.FromResult(0);
                },
                SecurityTokenValidated = (context) =>
                {
                    Logger.Debug("*** SecurityTokenValidated");
                    return Task.FromResult(0);
                },
                AuthorizationCodeReceived = async notification => { await AccessTokenHandler.GetAccessToken(notification); },
                AuthenticationFailed = (context) =>
                {
                    Logger.Debug("*** AuthenticationFailed");
                    return Task.FromResult(0);
                },
            };

        }
    }
}