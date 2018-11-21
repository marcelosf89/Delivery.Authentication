using System;

namespace Delivery.Authentication.Api.Models
{
    public class AccountOptions
    {
        public static TimeSpan RememberMeLoginDuration = TimeSpan.FromDays(30);

        public static bool ShowLogoutPrompt = true;

        // specify the Windows authentication scheme being used
        public static readonly string WindowsAuthenticationSchemeName = Microsoft.AspNetCore.Server.IISIntegration.IISDefaults.AuthenticationScheme;

        public static string InvalidCredentialsErrorMessage = "Invalid username or password";
    }
}
