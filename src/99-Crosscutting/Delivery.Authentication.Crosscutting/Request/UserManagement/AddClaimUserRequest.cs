using System;

namespace Delivery.Authentication.Crosscutting.Request.UserManagement
{
    public class AddClaimUserRequest
    {
        public string Username { get; set; }
        public string[] Claims { get; set; }
    }
}
