using System.Configuration;
using System.Web.Http;
using IdentityServer3.AccessTokenValidation;
using log4net;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(IdentityServerResourceAPI.Startup))]

namespace IdentityServerResourceAPI
{
    public class Startup
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void Configuration(IAppBuilder app)
        {
            log4net.Config.XmlConfigurator.Configure();

            app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
            {
                Authority = ConfigurationManager.AppSettings["auth:TokenValidationUrl"],  
                RequiredScopes = new[] { "sampleApi" }
            });

            var config = new HttpConfiguration();
            config.EnableCors();
            config.MapHttpAttributeRoutes();
            app.UseWebApi(config);
        }
    }
}
