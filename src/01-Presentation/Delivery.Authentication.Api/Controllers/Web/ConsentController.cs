using IdentityServer4.Events;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using Delivery.Authentication.Api.Models;
using Delivery.Authentication.Api.Models.Extensions;

namespace Delivery.Authentication.Api.Controllers.Web
{
    /// <summary>
    /// This controller processes the consent UI
    /// </summary>
    [SecurityHeaders]
    [Authorize]
    public class ConsentController : Controller
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly IResourceStore _resourceStore;
        private readonly IEventService _events;
        private readonly ILogger<ConsentController> _logger;

        public ConsentController(
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IResourceStore resourceStore,
            IEventService events,
            ILogger<ConsentController> logger)
        {
            _interaction = interaction;
            _clientStore = clientStore;
            _resourceStore = resourceStore;
            _events = events;
            _logger = logger;
        }

        /// <summary>
        /// Shows the consent screen
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Index(string returnUrl)
        {
            var vm = await BuildViewModelAsync(returnUrl);
            if (vm != null)
            {
                return View("Index", vm);
            }

            return View("Error");
        }

        /// <summary>
        /// Handles the consent screen postback
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ConsentViewModel model)
        {
            var result = await ProcessConsent(model);

            if (result.IsRedirect)
            {
                if (await _clientStore.IsPkceClientAsync(result.ClientId))
                {
                    // if the client is PKCE then we assume it's native, so this change in how to
                    // return the response is for better UX for the end user.
                    return View("Redirect", result.RedirectUri);
                }

                return Redirect(result.RedirectUri);
            }

            if (result.HasValidationError)
            {
                ModelState.AddModelError("", result.ValidationError);
            }

            return View("Error");
        }

        /*****************************************/
        /* helper APIs for the ConsentController */
        /*****************************************/
        private async Task<ProcessConsentResult> ProcessConsent(ConsentViewModel model)
        {
            // validate return url is still valid
            var request = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);
            if (request == null) return null;

            ConsentResponse grantedConsent = null;

            if (model.Button == "no")
            {
                grantedConsent = await ResponseNo(request);
            }
            else if (model.Button == "yes" && model != null)
            {
                if (model.ScopesConsented == null || !model.ScopesConsented.Any())
                {
                    return ProcessConsentResult.MustChooseOneErrorMessage;
                }
                grantedConsent = await ResponseYes(model, request);
            }
            else
            {
                return ProcessConsentResult.InvalidSelectionErrorMessage;
            }

            return await BuildProcessConsentResultAsync(grantedConsent, model, request); ;
        }

        private async Task<ProcessConsentResult> BuildProcessConsentResultAsync(ConsentResponse grantedConsent, ConsentViewModel model, AuthorizationRequest request)
        {
            // communicate outcome of consent back to identityserver
            await _interaction.GrantConsentAsync(request, grantedConsent);

            // indicate that's it ok to redirect back to authorization endpoint
            return new ProcessConsentResult(model.ReturnUrl, request.ClientId);
        }

        private async Task<ConsentResponse> ResponseYes(ConsentViewModel model, AuthorizationRequest request)
        {
            var scopes = model.ScopesConsented;
            if (!ConsentOptions.EnableOfflineAccess)
            {
                scopes = scopes.Where(x => x != IdentityServer4.IdentityServerConstants.StandardScopes.OfflineAccess);
            }

            ConsentResponse grantedConsent = new ConsentResponse
            {
                ScopesConsented = scopes
            };

            // emit event
            await _events.RaiseAsync(new ConsentGrantedEvent(User.GetSubjectId(), request.ClientId, request.ScopesRequested, grantedConsent.ScopesConsented, grantedConsent.RememberConsent));

            return grantedConsent;
        }

        private async Task<ConsentResponse> ResponseNo(AuthorizationRequest request)
        {
            ConsentResponse grantedConsent = ConsentResponse.Denied;
            await _events.RaiseAsync(new ConsentDeniedEvent(User.GetSubjectId(), request.ClientId, request.ScopesRequested));

            return grantedConsent;
        }

        private async Task<ConsentViewModel> BuildViewModelAsync(string returnUrl, ConsentViewModel model = null)
        {
            var request = await _interaction.GetAuthorizationContextAsync(returnUrl);
            if (request == null)
            {
                _logger.LogError("No consent request matching request: {0}", returnUrl);
                return null;
            }

            var client = await _clientStore.FindEnabledClientByIdAsync(request.ClientId);
            if (client == null)
            {
                _logger.LogError("Invalid client id: {0}", request.ClientId);
                return null;
            }

            var resources = await _resourceStore.FindEnabledResourcesByScopeAsync(request.ScopesRequested);
            if (resources != null && (resources.IdentityResources.Any() || resources.ApiResources.Any()))
            {
                return CreateConsentViewModel(model, returnUrl, client, resources);
            }

            _logger.LogError("No scopes matching: {0}", request.ScopesRequested.Aggregate((x, y) => x + ", " + y));
            return null;
        }

        private ConsentViewModel CreateConsentViewModel(ConsentViewModel model, string returnUrl, Client client, Resources resources)
        {
            ConsentViewModel vm = model ?? new ConsentViewModel
            {
                ScopesConsented = Enumerable.Empty<string>()
            };

            vm.ReturnUrl = returnUrl;
            vm.IdentityScopes = resources.IdentityResources.Select(x => x.CreateScopeViewModel(vm.ScopesConsented.Contains(x.Name) || model == null));
            vm.ResourceScopes = resources.ApiResources.SelectMany(x => x.Scopes).Select(x => x.CreateScopeViewModel(vm.ScopesConsented.Contains(x.Name) || model == null));

            if (ConsentOptions.EnableOfflineAccess && resources.OfflineAccess)
            {
                vm.ResourceScopes = vm.ResourceScopes.Union(new ScopeViewModel[] {
                    ScopeViewModel.Empty.GetOfflineAccessScope(vm.ScopesConsented.Contains(IdentityServer4.IdentityServerConstants.StandardScopes.OfflineAccess) || model == null)
                });
            }

            return vm;
        }
    }
}
