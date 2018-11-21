using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Delivery.Authentication.Api.Models
{
    [ExcludeFromCodeCoverage]
    public class ConsentViewModel
    {
        public string Button { get; set; }
        public IEnumerable<string> ScopesConsented { get; set; }
        public string ReturnUrl { get; set; }

        public IEnumerable<ScopeViewModel> IdentityScopes { get; set; }
        public IEnumerable<ScopeViewModel> ResourceScopes { get; set; }
    }
}
