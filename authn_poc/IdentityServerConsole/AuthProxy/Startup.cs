using IdentityServer3.Core.Configuration;
using log4net;
using Microsoft.Owin;
using Owin;
using ConfigManager = System.Configuration.ConfigurationManager;

[assembly: OwinStartup(typeof(AuthProxy.Startup))]

namespace AuthProxy
{
    public class Startup
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public void Configuration(IAppBuilder app)
        {
            log4net.Config.XmlConfigurator.Configure();
            var certificate = CertLoader.LoadCertificate();

            app.Map("/identity", idsrvApp =>
            {
                idsrvApp.UseIdentityServer(new IdentityServerOptions
                {
                    SiteName = ConfigManager.AppSettings["idsvr:siteName"],
                    SigningCertificate = certificate,
                    Factory = ServiceFactory.Build(),
                    AuthenticationOptions = AuthenticationOptionsFactory.Build(),
                    LoggingOptions = LoggingOptionsFactory.BuildLoggingOptions(),
                    EventsOptions = LoggingOptionsFactory.BuildEventsOptions(),
                    DataProtector = new X509CertificateDataProtector(certificate)
                });
            });

        }

    }
}
