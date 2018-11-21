using Cassandra;
using System;

namespace Delivery.Authentication.Infrastructure.Cassandra
{
    public interface ICassandraConnection
    {
        ISession GetSession();

        void Rollback(params Func<ISession, bool>[] actions);
    }
}
