using Delivery.Authentication.Domain.Model;
using Delivery.Authentication.Infrastructure.Cassandra.Constants;
using Delivery.Authentication.Infrastructure.Data.IdentityManager;
using Cassandra;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delivery.Authentication.Infrastructure.Cassandra.IdentityManagement
{
    public class IdentityClientDataCassandra : IIdentityClientData
    {
        private readonly ICassandraConnection _cassandraConnection;

        public IdentityClientDataCassandra(ICassandraConnection cassandraConnection)
        {
            _cassandraConnection = cassandraConnection;
        }

        public async Task<IdentityClient> GetIdentityClientByClientIdAsync(string clientId)
        {
            StringBuilder stringBuilder = new StringBuilder("SELECT ");
            stringBuilder.AppendJoin(", ",
                IdentityClientConstant.COLUMNS_CLIENT_ID,
                IdentityClientConstant.COLUMNS_CLIENT_DESCRIPTION,
                IdentityClientConstant.COLUMNS_TIME_LIFE,
                IdentityClientConstant.COLUMNS_GRANT_TYPES,
                IdentityClientConstant.COLUMNS_REQUIRE_CLIENT_SECRET,
                IdentityClientConstant.COLUMNS_CLIENT_SECRET,
                IdentityClientConstant.COLUMNS_ALLOW_ACCESS_IN_BROWSER,
                IdentityClientConstant.COLUMNS_SCOPES,
                IdentityClientConstant.COLUMNS_REDIRECT_URIS,
                IdentityClientConstant.COLUMNS_POST_LOGOUT_REDIRECT_URIS,
                IdentityClientConstant.COLUMNS_AUTHORITY,
                IdentityClientConstant.COLUMNS_ALLOW_OFFLINE_ACCESS,
                IdentityClientConstant.COLUMNS_CLAIMS);
            stringBuilder.Append(" from ");
            stringBuilder.Append(IdentityClientConstant.TABLE_NAME);
            stringBuilder.Append(" where ");
            stringBuilder.Append(IdentityClientConstant.COLUMNS_CLIENT_ID);
            stringBuilder.Append(" = ? ");

            SimpleStatement statement = new SimpleStatement(stringBuilder.ToString(), clientId);

            ISession session = _cassandraConnection.GetSession();

            RowSet rowSet = await session.ExecuteAsync(statement);
            Row row = rowSet.SingleOrDefault();

            return BuildIdentityClient(row);
        }

        private IdentityClient BuildIdentityClient(Row row)
        {
            if (row == null) return null;

            IdentityClient identityClient = new IdentityClient
            {
                ClientId = row.GetValue<string>(IdentityClientConstant.COLUMNS_CLIENT_ID),
                ClientDescription = row.GetValue<string>(IdentityClientConstant.COLUMNS_CLIENT_DESCRIPTION),
                TimeLife = row.GetValue<int>(IdentityClientConstant.COLUMNS_TIME_LIFE),
                GrantTypes = row.GetValue<ICollection<string>>(IdentityClientConstant.COLUMNS_GRANT_TYPES),
                RequireClientSecret = row.GetValue<bool>(IdentityClientConstant.COLUMNS_REQUIRE_CLIENT_SECRET),
                ClientSecret = row.GetValue<string>(IdentityClientConstant.COLUMNS_CLIENT_SECRET),
                AllowAccessInBrowser = row.GetValue<bool>(IdentityClientConstant.COLUMNS_ALLOW_ACCESS_IN_BROWSER),
                Scopes = row.GetValue<ICollection<string>>(IdentityClientConstant.COLUMNS_SCOPES),
                RedirectUris = row.GetValue<ICollection<string>>(IdentityClientConstant.COLUMNS_REDIRECT_URIS),
                PostLogoutRedirectUris = row.GetValue<ICollection<string>>(IdentityClientConstant.COLUMNS_POST_LOGOUT_REDIRECT_URIS),
                Authority = row.GetValue<ICollection<string>>(IdentityClientConstant.COLUMNS_AUTHORITY),
                AllowOfflineAccess = row.GetValue<bool>(IdentityClientConstant.COLUMNS_ALLOW_OFFLINE_ACCESS),
                Claims = row.GetValue<ICollection<IdentityClaim>>(IdentityClientConstant.COLUMNS_CLAIMS)
            };

            return identityClient;
        }
    }
}
