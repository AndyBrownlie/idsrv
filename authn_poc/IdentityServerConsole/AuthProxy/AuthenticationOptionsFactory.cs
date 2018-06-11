using System;
using IdentityServer3.Core.Configuration;
using Owin;
using ConfigManager = System.Configuration.ConfigurationManager;

namespace AuthProxy
{
    public static class AuthenticationOptionsFactory
    {
        public static AuthenticationOptions Build()
        {
            return new AuthenticationOptions
            {
                CookieOptions = new CookieOptions
                {
                    ExpireTimeSpan =
                        TimeSpan.FromMinutes(
                            Convert.ToDouble(ConfigManager.AppSettings["IdSvr:ExpireTimeSpanMinutes"])),
                    SlidingExpiration = true,
                    IsPersistent = Convert.ToBoolean(ConfigManager.AppSettings["IdSvr:PersistCookieAcrossSessions"])
                },
                EnableLocalLogin = Convert.ToBoolean(ConfigManager.AppSettings["IdSvr:EnableLocalLogin"]),
                IdentityProviders = ConfigureIdentityProviders
            };
        }


        private static void ConfigureIdentityProviders(IAppBuilder app, string signInAsType)
        {
            //app.UseOpenIdConnectAuthentication(new OidcExternalIdentityProvider(ConfigManager.AppSettings["IdSvr:ExternalIdPPrefix"], signInAsType));
            app
                .UseOpenIdConnectAuthentication(new OidcExternalIdentityProvider("Auth0"))
                .UseOpenIdConnectAuthentication(new OidcExternalIdentityProvider("Ping"))
                .UseOpenIdConnectAuthentication(new OidcExternalIdentityProvider("AAD"));
        }


    }
}