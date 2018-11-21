using Delivery.Authentication.Infrastructure.Cassandra.IdentityManagement;
using Delivery.Authentication.Infrastructure.Cassandra.UserManagement;
using Delivery.Authentication.Infrastructure.Data;
using Delivery.Authentication.Infrastructure.Data.IdentityManager;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Delivery.Authentication.Infrastructure.Cassandra
{
    [ExcludeFromCodeCoverage]
   public static class CassandraConfigurationBuilder
    {
        public static void AddCassandra(this IServiceCollection services)
        {
            services.AddSingleton<ICassandraConnection, CassandraConnection>();
            services.AddSingleton<IUserData, UserDataCassandra>();
            services.AddSingleton<IClaimData, ClaimDataCassandra>();

            services.AddSingleton<IIdentityClientData, IdentityClientDataCassandra>();
            services.AddSingleton<IIdentityApiData, IdentityApiDataCassandra>();
        }
    }
}
