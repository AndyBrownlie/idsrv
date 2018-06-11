using System;
using System.Configuration;
using Microsoft.Owin.Security.Cookies;

namespace OIDCGatewayClient
{
    public class WebAppCookieAuthenticationOptions : CookieAuthenticationOptions
    {
        public WebAppCookieAuthenticationOptions()
        {
            AuthenticationType = ConfigurationManager.AppSettings["cookie:AuthenticationType"];
            CookieSecure = CookieSecureOption.Always;
            ExpireTimeSpan =
                TimeSpan.FromMinutes(
                    Convert.ToDouble(ConfigurationManager.AppSettings["cookie:expireTimeSpanMinutes"]));
            SlidingExpiration = true;
        }
    }
}
