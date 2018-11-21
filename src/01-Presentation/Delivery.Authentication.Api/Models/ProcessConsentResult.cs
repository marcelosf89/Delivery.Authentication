
using System.Diagnostics.CodeAnalysis;

namespace Delivery.Authentication.Api.Models
{
    [ExcludeFromCodeCoverage]
    public class ProcessConsentResult
    {
        public ProcessConsentResult(string redirectUri, string clientId)
        {
            RedirectUri = redirectUri;
            ClientId = clientId;
            HasValidationError = false;
        }

        public ProcessConsentResult(string error)
        {
            ValidationError = error;
            HasValidationError = true;
        }

        public bool IsRedirect => RedirectUri != null;
        public string RedirectUri { get; set; }
        public string ClientId { get; set; }

        public bool HasValidationError { get; private set; }
        public string ValidationError { get; private set; }


        private static readonly ProcessConsentResult _mustChooseOneErrorMessage = new ProcessConsentResult(ConsentOptions.MustChooseOneErrorMessage);
        private static readonly ProcessConsentResult _invalidSelectionErrorMessage = new ProcessConsentResult(ConsentOptions.InvalidSelectionErrorMessage);

        public static ref readonly ProcessConsentResult MustChooseOneErrorMessage => ref _mustChooseOneErrorMessage;
        public static ref readonly  ProcessConsentResult InvalidSelectionErrorMessage => ref _invalidSelectionErrorMessage;
    }
}
