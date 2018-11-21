using Delivery.Authentication.Domain.Model;
using Cassandra;
using System.Diagnostics.CodeAnalysis;

namespace Delivery.Authentication.Infrastructure.Cassandra
{
    [ExcludeFromCodeCoverage]
    public static class CassandraDefineTypes
    {
        public static void AddUDTType(this ISession session)
        {
            session.UserDefinedTypes.Define(UdtMap.For<IdentityClaim>());
        }
    }
}
