using Delivery.Authentication.Crosscutting.Exceptions;
using Delivery.Authentication.Crosscutting.Helper;
using Delivery.Authentication.Crosscutting.Request.UserManagement;
using Delivery.Authentication.Domain.Model;
using Delivery.Authentication.Infrastructure.Data;
using System.Net;
using System.Text;

namespace Delivery.Authentication.Application.Command.UserManager
{
    public class UpdateUserCommand : ICommand<UpdateUserRequest>
    {
        private readonly IUserData _userdata;
        private readonly IPasswordHash _passwordHash;

        public UpdateUserCommand(IUserData userdata, IPasswordHash passwordHash)
        {
            _userdata = userdata;
            _passwordHash = passwordHash;
        }

        public void Execute(UpdateUserRequest userRequest)
        {
            User user = _userdata.GetUser(userRequest.Id);

            if (user is null) throw new ResponseException(HttpStatusCode.BadRequest, $"The user id does not exist");

            user.Email = userRequest.Email;
            user.Username = userRequest.Username;
            user.FirstName = userRequest.FirstName;
            user.LastName = userRequest.LastName;
            user.Phone = userRequest.Phone;

            if (!string.IsNullOrWhiteSpace(userRequest.Password))
            {
                user.Password = _passwordHash.Converter(userRequest.Password, Encoding.ASCII);
            }

            _userdata.Update(user);
        }
    }
}
