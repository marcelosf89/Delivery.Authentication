using Delivery.Authentication.Domain.Model;
using Delivery.Authentication.Infrastructure.Data;
using Cassandra;
using System.Linq;
using System.Text;

namespace Delivery.Authentication.Infrastructure.Cassandra.UserManagement
{
    public class ClaimDataCassandra : IClaimData
    {
        private readonly ICassandraConnection _cassandraConnection;

        public ClaimDataCassandra(ICassandraConnection cassandraConnection)
        {
            _cassandraConnection = cassandraConnection;
        }

        public void SetObsolete(string code)
        {
            var statement = new SimpleStatement($"UPDATE claims SET isObsolete = :IsObsolete WHERE code = :Code", 
                new {
                    Code = code,
                    IsObsolete = true
                });

            _cassandraConnection.GetSession().Execute(statement);
        }

        public Claim GetClaim(string code)
        {
            var statement = new SimpleStatement("SELECT code, description, isobsolete from claims where code = ?", code);

            RowSet rowSet = _cassandraConnection.GetSession().Execute(statement);

            var row = rowSet.SingleOrDefault();

            return BuildClaim(row);
        }

        public bool HasClaim(string code)
        {
            var statement = new SimpleStatement("SELECT code from claims where code = ?", code);

            RowSet rowSet = _cassandraConnection.GetSession().Execute(statement);

            return rowSet.Any();
        }

        public void Save(Claim claim)
        {
            StringBuilder stringBuilderFields = new StringBuilder();
            stringBuilderFields.Append("INSERT INTO claims (");
            stringBuilderFields.AppendJoin(',', 
                                        nameof(claim.Code),
                                        nameof(claim.Description),
                                        nameof(claim.IsObsolete));
            stringBuilderFields.Append(") values(:");
            stringBuilderFields.AppendJoin(" ,:",
                                        nameof(claim.Code),
                                        nameof(claim.Description),
                                        nameof(claim.IsObsolete));
            stringBuilderFields.Append(")");
            ISession session = _cassandraConnection.GetSession();

            var statement = session.Prepare(stringBuilderFields.ToString());
            session.Execute(statement.Bind(new
            {
                claim.Code,
                claim.Description,
                claim.IsObsolete
            }));
        }

        public void Update(Claim claim)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat(" {0} = :{0},", nameof(claim.Description));

            ISession session = _cassandraConnection.GetSession();

            var statement = session.Prepare($"UPDATE claims SET {stringBuilder.ToString()} WHERE code = :Code");
            session.Execute(statement.Bind(new
            {
                claim.Code,
                claim.Description
            }));
        }

        private Claim BuildClaim(Row row)
        {
            if (row == null) return null;

            Claim claim = new Claim
            {
                Code = row.GetValue<string>("code"),
                Description = row.GetValue<string>("description"),
                IsObsolete = row.GetValue<bool>("isobsolete")
            };

            return claim;
        }
    }
}
