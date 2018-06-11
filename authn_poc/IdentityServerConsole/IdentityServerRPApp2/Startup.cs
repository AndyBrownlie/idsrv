using Microsoft.Owin;
using Owin;
using OIDCGatewayClient;


[assembly: OwinStartup(typeof(IdentityServerMVC.Startup))]

namespace IdentityServerMVC
{
    public class Startup
    {

        public void Configuration(IAppBuilder app)
        {
            app.UseCookieAuthentication(new WebAppCookieAuthenticationOptions());
            app.UseOpenIdConnectAuthentication(new WebAppOidcAuthenticationOptions());
        }
    }
}
