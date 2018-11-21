using Delivery.Authentication.Crosscutting.Response.UserManagement;
using Delivery.Authentication.Domain.Model;
using Delivery.Authentication.Infrastructure.Cassandra.Builders;
using Delivery.Authentication.Infrastructure.Cassandra.Constants;
using Delivery.Authentication.Infrastructure.Data;
using Cassandra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Delivery.Authentication.Infrastructure.Cassandra.UserManagement
{
    public class UserDataCassandra : IUserData
    {
        private readonly ICassandraConnection _cassandraConnection;
        public UserDataCassandra(ICassandraConnection cassandraConnection)
        {
            _cassandraConnection = cassandraConnection;
        }

        public void Delete(Guid id)
        {
            var statement = new SimpleStatement($"UPDATE { UserConstant.TABLE_NAME } SET {UserConstant.COLUMNS_DELETION_DATE} = :DeletionDate WHERE {UserConstant.COLUMNS_ID} = :Id",
                new
                {
                    Id = id,
                    DeletionDate = DateTime.UtcNow
                });

            _cassandraConnection.GetSession().Execute(statement);
        }

        public User GetUser(Guid id)
        {
            var statement = new SimpleStatement(
                $"SELECT {UserConstant.ALL_COLUMNS_EXCEPT_CLAIMS} from { UserConstant.TABLE_NAME } where {UserConstant.COLUMNS_ID} = ?", id);

            RowSet rowSet = _cassandraConnection.GetSession().Execute(statement);

            var row = rowSet.SingleOrDefault();

            return row.BuildUser();
        }

        public User GetUserByEmail(string email)
        {
            var statement = new SimpleStatement(
                $"SELECT {UserConstant.ALL_COLUMNS_EXCEPT_CLAIMS} from { UserConstant.TABLE_NAME } where {UserConstant.COLUMNS_EMAIL} = ? ALLOW FILTERING",
                email);

            RowSet rowSet = _cassandraConnection.GetSession().Execute(statement);

            var row = rowSet.SingleOrDefault();

            return row.BuildUser();
        }

        public User GetUserByUsername(string username)
        {
            var statement = new SimpleStatement(
                $"SELECT {UserConstant.ALL_COLUMNS_EXCEPT_CLAIMS} from { UserConstant.TABLE_NAME } where {UserConstant.COLUMNS_USERNAME} = ? ALLOW FILTERING",
                username);

            RowSet rowSet = _cassandraConnection.GetSession().Execute(statement);

            var row = rowSet.SingleOrDefault();

            return row.BuildUser();
        }

        public void Save(User user)
        {
            StringBuilder stringBuilderFields = new StringBuilder();
            stringBuilderFields.Append($"INSERT INTO { UserConstant.TABLE_NAME } (");
            stringBuilderFields.AppendJoin(',',
                                        UserConstant.COLUMNS_ID,
                                        UserConstant.COLUMNS_EMAIL,
                                        UserConstant.COLUMNS_USERNAME,
                                        UserConstant.COLUMNS_FIRST_NAME,
                                        UserConstant.COLUMNS_LAST_NAME,
                                        UserConstant.COLUMNS_PASSWORD,
                                        UserConstant.COLUMNS_PHONE,
                                        UserConstant.COLUMNS_CREATION,
                                        UserConstant.COLUMNS_LAST_ACCESS,
                                        UserConstant.COLUMNS_DELETION_DATE,
                                        UserConstant.COLUMNS_CLAIMS);
            stringBuilderFields.Append(") values(:");
            stringBuilderFields.AppendJoin(" ,:",
                                        nameof(user.Id),
                                        nameof(user.Email),
                                        nameof(user.Username),
                                        nameof(user.FirstName),
                                        nameof(user.LastName),
                                        nameof(user.Password),
                                        nameof(user.Phone),
                                        nameof(user.Creation),
                                        nameof(user.LastAccess),
                                        nameof(user.DeletionDate),
                                        nameof(user.Claims));
            stringBuilderFields.Append(")");
            ISession session = _cassandraConnection.GetSession();

            var statement = session.Prepare(stringBuilderFields.ToString());
            session.Execute(statement.Bind(new
            {
                user.Id,
                user.Email,
                user.FirstName,
                user.LastName,
                user.Username,
                user.Password,
                user.Phone,
                user.Creation,
                user.LastAccess,
                user.DeletionDate,
                user.Claims
            }));
        }

        public void Update(User user)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendFormat(" {0} = :{0},", nameof(user.Email));
            stringBuilder.AppendFormat(" {0} = :{0},", nameof(user.FirstName));
            stringBuilder.AppendFormat(" {0} = :{0},", nameof(user.LastName));
            stringBuilder.AppendFormat(" {0} = :{0},", nameof(user.Password));
            stringBuilder.AppendFormat(" {0} = :{0},", nameof(user.Phone));
            stringBuilder.AppendFormat(" {0} = :{0},", nameof(user.DeletionDate));
            stringBuilder.AppendFormat(" {0} = :{0}", nameof(user.Username));

            ISession session = _cassandraConnection.GetSession();

            var statement = session.Prepare($"UPDATE { UserConstant.TABLE_NAME } SET {stringBuilder.ToString()} WHERE {UserConstant.COLUMNS_ID} = :Id");
            session.Execute(statement.Bind(new
            {
                user.Id,
                user.Email,
                user.FirstName,
                user.LastName,
                user.Password,
                user.Phone,
            }));
        }

        public void UpdeteClaims(Guid id, params string[] claims)
        {
            ISession session = _cassandraConnection.GetSession();
            var statement = session.Prepare(
                $"UPDATE { UserConstant.TABLE_NAME } SET { UserConstant.COLUMNS_CLAIMS } = :claims  where id = :id ALLOW FILTERING");

            session.Execute(statement.Bind(new
            {
                id, claims
            }));
        }

        public UserAuthInfoResponse GetUserAuthInfo(string username, string password)
        {
            var statement = new SimpleStatement(
                $"SELECT {UserConstant.ALL_COLUMNS_EXCEPT_CLAIMS} from { UserConstant.TABLE_NAME } where username = ? and password = ? ALLOW FILTERING",
                username, password);

            RowSet rowSet = _cassandraConnection.GetSession().Execute(statement);

            var row = rowSet.SingleOrDefault();

            return row.BuildUserAuthInfoResponse();
        }

        public IEnumerable<string> GetClaimsByUserId(Guid userId)
        {
            var statement = new SimpleStatement(
                $"SELECT {UserConstant.COLUMNS_CLAIMS} from { UserConstant.TABLE_NAME } where {UserConstant.COLUMNS_ID } = ? ", userId);

            RowSet rowSet = _cassandraConnection.GetSession().Execute(statement);

            var row = rowSet.SingleOrDefault();

            return row.GetValue<ICollection<string>>(UserConstant.COLUMNS_CLAIMS);
        }


    }
}
