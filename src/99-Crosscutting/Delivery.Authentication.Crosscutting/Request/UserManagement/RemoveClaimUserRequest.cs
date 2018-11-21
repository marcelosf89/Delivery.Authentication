using System;

namespace Delivery.Authentication.Crosscutting.Request.UserManagement
{
    public class RemoveClaimUserRequest
    {
        public string Username { get; set; }
        public string[] Claims { get; set; }
    }
}
