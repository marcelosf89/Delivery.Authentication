using System.Diagnostics.CodeAnalysis;

namespace Delivery.Authentication.Api.Models
{
    [ExcludeFromCodeCoverage]
    public class LogoutViewModel
    {
        public string LogoutId { get; set; }
        public bool ShowLogoutPrompt { get; set; } = true;
    }
}
