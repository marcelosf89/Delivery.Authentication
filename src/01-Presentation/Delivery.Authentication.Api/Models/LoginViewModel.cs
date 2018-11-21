using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Delivery.Authentication.Api.Models
{
    [ExcludeFromCodeCoverage]
    public class LoginViewModel
    {
        private static IEnumerable<ExternalProvider> _externalProviders = new List<ExternalProvider>();

        public LoginViewModel()
        {
            ExternalProviders = _externalProviders;
        }

        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
        public bool EnableLocalLogin { get; set; } = true;


        public IEnumerable<ExternalProvider> ExternalProviders { get; set; }
        public IEnumerable<ExternalProvider> VisibleExternalProviders => ExternalProviders.Where(x => !string.IsNullOrWhiteSpace(x.DisplayName));

        public bool IsExternalLoginOnly => !EnableLocalLogin && ExternalProviders.Any();
        public string ExternalLoginScheme => IsExternalLoginOnly ? ExternalProviders.SingleOrDefault()?.AuthenticationScheme : null;
    }
}
