using System;

namespace Delivery.Authentication.Domain.Model
{
    public class Claim
    {
        public string Code { get; set; }

        public string Description { get; set; }

        public bool IsObsolete { get; set; }
    }
}
