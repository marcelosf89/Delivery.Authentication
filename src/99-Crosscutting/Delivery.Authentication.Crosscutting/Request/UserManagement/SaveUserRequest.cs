
using System.Collections.Generic;

namespace Delivery.Authentication.Crosscutting.Request.UserManagement
{
    public class SaveUserRequest
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public IEnumerable<string> Claims { get; set; }
    }
}
