using Delivery.Authentication.Application.Command.UserManager.Models;
using Delivery.Authentication.Crosscutting.Exceptions;
using Delivery.Authentication.Domain.Model;
using Delivery.Authentication.Infrastructure.Data;
using System.Net;
using System.Linq;
using System.Collections.Generic;

namespace Delivery.Authentication.Application.Command.UserManager
{
    public class AddClaimUserCommnad : ICommand<AddClaimUserCommnadRequest>
    {
        private readonly IUserData _userData;

        public AddClaimUserCommnad(IUserData userData)
        {
            _userData = userData;
        }

        public void Execute(AddClaimUserCommnadRequest request)
        {
            if (request.Claims is null)
                return;

            User user = _userData.GetUserByUsername(request.Username);

            if (user is null)
                throw new ResponseException(HttpStatusCode.BadRequest, $"The username does not exist");

            if (user.Claims is null)
                user.Claims = new HashSet<string>();

            foreach (var claim in request.Claims)
            {
                if(!user.Claims.Contains(claim.ToLower()))
                {
                    user.Claims.Add(claim.ToLower());
                }
            }

            _userData.UpdeteClaims(user.Id, user.Claims.ToArray());
        }
    }
}
