using System;
using System.Collections.Generic;

namespace Delivery.Authentication.Domain.Model
{
    public class User
    {
        public Guid Id { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Phone { get; set; }

        public string Password { get; set; }

        public DateTime Creation { get; set; }

        public DateTime LastAccess { get; set; }

        public DateTime? DeletionDate { get; set; }

        public HashSet<string> Claims { get; set; }
    }
}
