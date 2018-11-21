using System;
using System.Collections.Generic;

namespace Delivery.Authentication.Application.Command.UserManager.Models
{
    public class AddClaimUserCommnadRequest
    {
        public string Username { get; set; }
        public IEnumerable<string> Claims { get; set; }
    }
}
