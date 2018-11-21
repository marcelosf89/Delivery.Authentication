using Delivery.Authentication.Application.Query.UserManager.Models;
using Delivery.Authentication.Crosscutting.Exceptions;
using Delivery.Authentication.Domain.Model;
using Delivery.Authentication.Infrastructure.Data;

namespace Delivery.Authentication.Application.Query.UserManager
{
    public class GetUserByIdQuery : IQuery<GetUserByIdQueryRequest, User>
    {
        private readonly IUserData _userData;

        public GetUserByIdQuery(IUserData userData)
        {
            _userData = userData;
        }

        public User Execute(GetUserByIdQueryRequest getUserByIdQuery)
        {
            User user = _userData.GetUser(getUserByIdQuery.Id);

            if (user == null)
            {
                throw new ResponseException(System.Net.HttpStatusCode.BadRequest, $"The user does not exist");
            }

            return user;
        }
    }
}
