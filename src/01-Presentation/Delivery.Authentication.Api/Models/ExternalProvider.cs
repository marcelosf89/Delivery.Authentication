using System.Diagnostics.CodeAnalysis;

namespace Delivery.Authentication.Api.Models
{
    [ExcludeFromCodeCoverage]
    public class ExternalProvider
    {
        public string DisplayName { get; set; }
        public string AuthenticationScheme { get; set; }
    }
}
