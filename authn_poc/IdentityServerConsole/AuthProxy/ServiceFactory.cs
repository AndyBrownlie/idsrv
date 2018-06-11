using IdentityServer3.Contrib.Cache.Redis;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Models;
using IdentityServer3.Core.Services;
using IdentityServer3.Core.Services.InMemory;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Security.Claims;

namespace AuthProxy
{
    public static class ServiceFactory
    {
        public static IdentityServerServiceFactory Build()
        {
            var factory = new IdentityServerServiceFactory();
            //factory.ConfigureRedisServices();
            factory.ConfigureInMemoryServices();
            factory.EventService = new Registration<IEventService, EventLoggingService>();
            return factory;
        }
    }


    internal static class ServiceFactoryExtensions
    {
        private static readonly string CacheConnection = ConfigurationManager.AppSettings["IdSvr:CacheConnection"];
        private static readonly Lazy<ConnectionMultiplexer> LazyConnection =
            new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(CacheConnection));

        public static IdentityServerServiceFactory ConfigureRedisServices(this IdentityServerServiceFactory factory)
        {
            factory.ClientStore = new Registration<IClientStore>(new InMemoryClientStore(ClientsFactory.Build()));
            factory.ScopeStore = new Registration<IScopeStore>(new InMemoryScopeStore(ScopesFactory.Build()));
            factory.UserService = new Registration<IUserService>(new InMemoryUserService(UsersFactory.Get()));
            factory.ConfigureClientStoreCache(new Registration<ICache<Client>>(new ClientStoreCache(LazyConnection.Value)));
            factory.ConfigureScopeStoreCache(new Registration<ICache<IEnumerable<Scope>>>(new ScopeStoreCache(LazyConnection.Value)));
            factory.ConfigureUserServiceCache(new Registration<ICache<IEnumerable<Claim>>>(new UserServiceCache(LazyConnection.Value)));
            factory.ConfigureOperationalRedisStoreServices(CacheConnection);
            return factory;
        }

        internal static IdentityServerServiceFactory ConfigureInMemoryServices(this IdentityServerServiceFactory factory)
        {
            factory.UseInMemoryUsers(UsersFactory.Get())
                .UseInMemoryClients(ClientsFactory.Build())
                .UseInMemoryScopes(ScopesFactory.Build());
            return factory;
        }

    }

}