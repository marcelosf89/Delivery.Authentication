using Delivery.Authentication.Application.Command.UserManager.Models;
using Delivery.Authentication.Crosscutting.Exceptions;
using Delivery.Authentication.Crosscutting.Helper;
using Delivery.Authentication.Domain.Model;
using Delivery.Authentication.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace Delivery.Authentication.Application.Command.UserManager
{
    public class RemoveClaimUserCommnad : ICommand<RemoveClaimUserCommnadRequest>
    {
        private readonly IUserData _userData;

        public RemoveClaimUserCommnad(IUserData userData)
        {
            _userData = userData;
        }

        public void Execute(RemoveClaimUserCommnadRequest request)
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
                if (user.Claims.Contains(claim.ToLower()))
                {
                    user.Claims.Remove(claim.ToLower());
                }
            }

            _userData.UpdeteClaims(user.Id, user.Claims.ToArray());
        }
    }
}
