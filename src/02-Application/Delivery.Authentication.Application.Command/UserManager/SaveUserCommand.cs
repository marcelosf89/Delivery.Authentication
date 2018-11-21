using Delivery.Authentication.Crosscutting.Exceptions;
using Delivery.Authentication.Crosscutting.Helper;
using Delivery.Authentication.Crosscutting.Request.UserManagement;
using Delivery.Authentication.Domain.Model;
using Delivery.Authentication.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Delivery.Authentication.Application.Command.UserManager
{
    public class SaveUserCommand : ICommand<SaveUserRequest>
    {
        private readonly IUserData _userdata;
        private readonly IPasswordHash _passwordHash;

        public SaveUserCommand(IUserData userdata, IPasswordHash passwordHash)
        {
            _userdata = userdata;
            _passwordHash = passwordHash;
        }

        public void Execute(SaveUserRequest userRequest)
        {
            userRequest.Email = userRequest.Email.ToLower();
            userRequest.Username = userRequest.Username.ToLower();

            CheckIfUserExist(userRequest);

            User user = new User
            {
                Creation = DateTime.UtcNow,
                Email = userRequest.Email,
                FirstName = userRequest.FirstName,
                LastName = userRequest.LastName,
                Phone = userRequest.Phone,
                Username = userRequest.Username,
                Id = Guid.NewGuid(),
                LastAccess = DateTime.UtcNow,
                Claims = new HashSet<string>()
            };

            if (userRequest.Claims != null)
            {
                foreach (var claim in userRequest.Claims)
                {
                    user.Claims.Add(claim.ToLower());
                }
            }

            user.Password = _passwordHash.Converter(userRequest.Password, Encoding.ASCII);

            _userdata.Save(user);
        }

        private void CheckIfUserExist(SaveUserRequest userRequest)
        {
            User user = _userdata.GetUserByUsername(userRequest.Username);
            if (user != null)
            {
                throw new ResponseException(HttpStatusCode.BadRequest, "The username already exists");
            }

            user = _userdata.GetUserByEmail(userRequest.Email); // TODO: Remove this options or add in elastic search to find all field username or email
            if (user != null)
            {
                throw new ResponseException(HttpStatusCode.BadRequest, "The email already exists");
            }
        }
    }
}