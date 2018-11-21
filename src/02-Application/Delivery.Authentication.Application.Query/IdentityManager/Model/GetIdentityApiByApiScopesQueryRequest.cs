using System;
using System.Collections.Generic;
using System.Text;

namespace Delivery.Authentication.Application.Query.IdentityManager.Model
{
    public class GetIdentityApiByApiScopesQueryRequest
    {
        public string[] ApiScope { get; set; }
    }
}
