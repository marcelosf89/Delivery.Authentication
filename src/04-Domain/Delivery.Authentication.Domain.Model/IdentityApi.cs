using System.Collections.Generic;

namespace Delivery.Authentication.Domain.Model
{
    public class IdentityApi
    {
        public string Code { get; set; }
        public string Description { get; set; }
        public ICollection<string> Claims { get; set; }
    }
}
