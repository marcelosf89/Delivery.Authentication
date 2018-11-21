using System;
using System.Collections.Generic;
using System.Text;

namespace Delivery.Authentication.Crosscutting.Response.UserManagement
{
    public class UserAuthInfoResponse
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public IEnumerable<string> Claims { get; set; }
    }
}
