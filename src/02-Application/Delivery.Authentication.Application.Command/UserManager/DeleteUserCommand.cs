using Delivery.Authentication.Application.Command.UserManager.Models;
using Delivery.Authentication.Crosscutting.Exceptions;
using Delivery.Authentication.Crosscutting.Helper;
using Delivery.Authentication.Domain.Model;
using Delivery.Authentication.Infrastructure.Data;
using System.Net;

namespace Delivery.Authentication.Application.Command.UserManager
{
    public class DeleteUserCommand : ICommand<DeleteUserCommnadRequest>
    {
        private readonly IUserData _userdata;

        public DeleteUserCommand(IUserData userdata)
        {
            _userdata = userdata;
        }

        public void Execute(DeleteUserCommnadRequest deleteUserCommnad)
        {
            User user = _userdata.GetUser(deleteUserCommnad.Id);

            if (user is null) throw new ResponseException(HttpStatusCode.BadRequest, $"The user id does not exist");

            _userdata.Delete(deleteUserCommnad.Id);
        }
    }
}
