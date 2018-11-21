using Delivery.Authentication.Domain.Model;
using Delivery.Authentication.Infrastructure.Cassandra.Constants;
using Delivery.Authentication.Infrastructure.Data.IdentityManager;
using Cassandra;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Delivery.Authentication.Infrastructure.Cassandra.IdentityManagement
{
    public class IdentityApiDataCassandra : IIdentityApiData
    {
        private readonly ICassandraConnection _cassandraConnection;

        public IdentityApiDataCassandra(ICassandraConnection cassandraConnection)
        {
            _cassandraConnection = cassandraConnection;
        }

        public async Task<IEnumerable<IdentityApi>> GetIdentityApiByApiScopesAsync(string[] apiScope)
        {
            StringBuilder stringBuilder = new StringBuilder("SELECT ");
            stringBuilder.AppendJoin(", ", IdentityApiConstant.COLUMNS_CODE, IdentityApiConstant.COLUMNS_DESCRIPTION, IdentityApiConstant.COLUMNS_CLAIMS);
            stringBuilder.Append(" from ");
            stringBuilder.Append(IdentityApiConstant.TABLE_NAME);
            stringBuilder.Append(" where ");
            stringBuilder.Append(IdentityApiConstant.COLUMNS_CODE);
            stringBuilder.Append(" in (");

            for (int i = 0; i < apiScope.Length; i++)
            {
                stringBuilder.Append("?");
                stringBuilder.Append(",");
            }
            stringBuilder.Remove(stringBuilder.Length - 1, 1);

            stringBuilder.Append(")");

            SimpleStatement statement = new SimpleStatement(stringBuilder.ToString(), apiScope);

            ISession session = _cassandraConnection.GetSession();

            RowSet rowSet = await session.ExecuteAsync(statement);

            return GetList(rowSet);
        }

        private IEnumerable<IdentityApi> GetList(RowSet rowSet)
        {
            foreach (Row row in rowSet)
            {
                yield return BuildIdentityClient(row);
            }
        }

        private IdentityApi BuildIdentityClient(Row row)
        {
            if (row == null) return null;

            IdentityApi identityApi = new IdentityApi
            {
                Code = row.GetValue<string>(IdentityApiConstant.COLUMNS_CODE),
                Description = row.GetValue<string>(IdentityApiConstant.COLUMNS_DESCRIPTION),
                Claims = row.GetValue<ICollection<string>>(IdentityApiConstant.COLUMNS_CLAIMS)
            };

            return identityApi;
        }
    }
}
