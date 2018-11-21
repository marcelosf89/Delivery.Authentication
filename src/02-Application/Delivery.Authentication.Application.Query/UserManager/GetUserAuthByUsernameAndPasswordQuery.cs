using Delivery.Authentication.Application.Query.UserManager.Models;
using Delivery.Authentication.Crosscutting.Helper;
using Delivery.Authentication.Crosscutting.Response.UserManagement;
using Delivery.Authentication.Infrastructure.Data;
using System.Text;

namespace Delivery.Authentication.Application.Query.UserManager
{
    public class GetUserAuthByUsernameAndPasswordQuery : IQuery<GetUserAuthByUsernameAndPasswordQueryRequest, UserAuthInfoResponse>
    {
        private readonly IUserData _userdata;
        private readonly IPasswordHash _passwordHash;

        public GetUserAuthByUsernameAndPasswordQuery(IUserData userdata, IPasswordHash passwordHash)
        {
            _userdata = userdata;
            _passwordHash = passwordHash;
        }

        public UserAuthInfoResponse Execute(GetUserAuthByUsernameAndPasswordQueryRequest request)
        {
            request.Password = _passwordHash.Converter(request.Password, Encoding.ASCII);

            UserAuthInfoResponse userAccess = _userdata.GetUserAuthInfo(request.Username, request.Password);

            return userAccess;
        }
    }
}
