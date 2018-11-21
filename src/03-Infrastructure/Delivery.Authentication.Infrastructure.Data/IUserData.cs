using Delivery.Authentication.Crosscutting.Response.UserManagement;
using Delivery.Authentication.Domain.Model;
using System;
using System.Collections.Generic;

namespace Delivery.Authentication.Infrastructure.Data
{
    public interface IUserData
    {
        User GetUser(Guid id);

        User GetUserByEmail(string email);

        User GetUserByUsername(string username);

        void Update(User user);

        void UpdeteClaims(Guid id, params string[] claims);

        void Save(User user);

        void Delete(Guid id);

        UserAuthInfoResponse GetUserAuthInfo(string username, string password);

        IEnumerable<string> GetClaimsByUserId(Guid userId);
    }
}
