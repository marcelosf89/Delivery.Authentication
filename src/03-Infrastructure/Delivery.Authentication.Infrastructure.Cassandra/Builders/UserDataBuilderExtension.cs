using Delivery.Authentication.Crosscutting.Response.UserManagement;
using Delivery.Authentication.Domain.Model;
using Delivery.Authentication.Infrastructure.Cassandra.Constants;
using Cassandra;
using System;
using System.Collections.Generic;

namespace Delivery.Authentication.Infrastructure.Cassandra.Builders
{
    internal static class UserDataBuilderExtension
    {
        internal static  User BuildUser(this Row row)
        {
            if (row == null) return null;

            User user = new User
            {
                Id = row.GetValue<Guid>(UserConstant.COLUMNS_ID),
                Email = row.GetValue<string>(UserConstant.COLUMNS_EMAIL),
                Username = row.GetValue<string>(UserConstant.COLUMNS_USERNAME),
                FirstName = row.GetValue<string>(UserConstant.COLUMNS_FIRST_NAME),
                LastName = row.GetValue<string>(UserConstant.COLUMNS_LAST_NAME),
                Creation = row.GetValue<DateTime>(UserConstant.COLUMNS_CREATION),
                LastAccess = row.GetValue<DateTime>(UserConstant.COLUMNS_LAST_ACCESS),
                DeletionDate = row.GetValue<DateTime?>(UserConstant.COLUMNS_DELETION_DATE),
                Phone = row.GetValue<string>(UserConstant.COLUMNS_PHONE),
            };

            return user;
        }

        internal static UserAuthInfoResponse BuildUserAuthInfoResponse(this Row row)
        {
            if (row == null) return null;

            UserAuthInfoResponse user = new UserAuthInfoResponse
            {
                Id = row.GetValue<Guid>(UserConstant.COLUMNS_ID),
                Email = row.GetValue<string>(UserConstant.COLUMNS_EMAIL),
                Username = row.GetValue<string>(UserConstant.COLUMNS_USERNAME),
                FirstName = row.GetValue<string>(UserConstant.COLUMNS_FIRST_NAME),
                LastName = row.GetValue<string>(UserConstant.COLUMNS_LAST_NAME),
                Claims = row.GetValue<IEnumerable<string>>(UserConstant.COLUMNS_CLAIMS)
            };

            return user;
        }
    }
}
