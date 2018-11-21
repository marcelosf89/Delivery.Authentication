using System.Collections.Generic;

namespace Delivery.Authentication.Domain.Model
{
    public class IdentityClient
    {
        public string ClientId { get; set; }
        public string ClientDescription { get; set; }
        public int TimeLife { get; set; }
        public ICollection<string> GrantTypes { get; set; }
        public bool RequireClientSecret { get; set; }
        public string ClientSecret { get; set; }
        public bool AllowAccessInBrowser { get; set; }
        public ICollection<string> Scopes { get; set; }
        public ICollection<string> RedirectUris { get; set; }
        public ICollection<string> PostLogoutRedirectUris { get; set; }
        public ICollection<string> Authority { get; set; }
        public bool AllowOfflineAccess { get; set; }
        public ICollection<IdentityClaim> Claims { get; set; }
    }
}
