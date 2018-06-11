using System.Threading.Tasks;
using IdentityServer3.Core.Events;
using IdentityServer3.Core.Services;
using log4net;

namespace AuthProxy
{
    public class EventLoggingService : IEventService
    {
        private static readonly ILog Logger;

        static EventLoggingService()
        {
            Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        }

        public Task RaiseAsync<T>(Event<T> evt)
        {
            Logger.Info($"({evt.EventType}) - {evt.Id}: {evt.Name} / {evt.Category}, Context: {evt.Context}, Details: {evt.Details}");

            return Task.FromResult(0);
        }
    }
}