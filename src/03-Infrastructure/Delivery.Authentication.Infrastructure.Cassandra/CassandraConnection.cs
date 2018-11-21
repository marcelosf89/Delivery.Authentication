using Delivery.Authentication.Crosscutting.Configuration;
using Cassandra;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace Delivery.Authentication.Infrastructure.Cassandra
{
    [ExcludeFromCodeCoverage]
    public class CassandraConnection : ICassandraConnection, IDisposable
    {
        private ref readonly Lazy<ISession> GetSessionLazyAOT() => ref _sessionLazy;

        private readonly CassandraConnectionSetting _setting;
        private readonly Lazy<ISession> _sessionLazy;
        private readonly Lazy<Cluster> _clusterLazy;

        public CassandraConnection(IOptions<CassandraConnectionSetting> setting)
        {
            _setting = setting.Value;

            _clusterLazy = new Lazy<Cluster>(() => { return Connect(); }, LazyThreadSafetyMode.ExecutionAndPublication);
            _sessionLazy = new Lazy<ISession>(() => { return CreateSessionLazy(); }, LazyThreadSafetyMode.ExecutionAndPublication);
        }

        private Cluster Connect()
        {
            return Cluster
              .Builder()
              .AddContactPoints(_setting.Hosts)
              .WithCredentials(_setting.User, _setting.Password)
              .WithPort(_setting.Port)
              .Build();
        }

        public ISession GetSession() => GetSessionLazyAOT().Value;

        public ISession CreateSessionLazy()
        {
            ISession session = _clusterLazy.Value.Connect(_setting.Keyspace);

            session.AddUDTType();

            return session;
        }

        public void Rollback(params Func<ISession, bool>[] actions)
        {
            for (int i = actions.Length; i > 0; i--)
            {
                if (!actions[i - 1](_sessionLazy.Value))
                {
                    //Save action to roolback with event-source;
                    return;
                }
            }
        }

        public void Dispose()
        {
            _sessionLazy.Value.Dispose();
            _clusterLazy.Value.Dispose();
        }
    }
}
