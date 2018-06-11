using System;
using System.Configuration;
using IdentityServer3.Core.Configuration;

namespace AuthProxy
{
    public static class LoggingOptionsFactory
    {
        public static LoggingOptions BuildLoggingOptions()
        {
            return new LoggingOptions
            {
                EnableHttpLogging = Convert.ToBoolean(ConfigurationManager.AppSettings["IdSvr:EnableHttpLogging"]), 
                EnableWebApiDiagnostics = Convert.ToBoolean(ConfigurationManager.AppSettings["IdSvr:EnableWebApiDiagnostics"]), 
                EnableKatanaLogging = Convert.ToBoolean(ConfigurationManager.AppSettings["IdSvr:EnableKatanaLogging"]), 
                WebApiDiagnosticsIsVerbose = Convert.ToBoolean(ConfigurationManager.AppSettings["IdSvr:WebApiDiagnosticsIsVerbose"]), 
            };
        }

        public static EventsOptions BuildEventsOptions()
        {
            return new EventsOptions
            {
                RaiseErrorEvents = Convert.ToBoolean(ConfigurationManager.AppSettings["IdSvr:RaiseErrorEvents"]),
                RaiseFailureEvents = Convert.ToBoolean(ConfigurationManager.AppSettings["IdSvr:RaiseFailureEvents"]),
                RaiseInformationEvents =
                    Convert.ToBoolean(ConfigurationManager.AppSettings["IdSvr:RaiseInformationEvents"]),
                RaiseSuccessEvents = Convert.ToBoolean(ConfigurationManager.AppSettings["IdSvr:RaiseSuccessEvents"])
            };
        }

    }
}