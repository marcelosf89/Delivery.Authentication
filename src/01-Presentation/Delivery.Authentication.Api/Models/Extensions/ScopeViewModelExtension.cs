using IdentityServer4.Models;

namespace Delivery.Authentication.Api.Models.Extensions
{
    public static class ScopeViewModelExtension
    {
        public static ScopeViewModel CreateScopeViewModel(this IdentityResource identity, bool check)
        {
            return new ScopeViewModel
            {
                Name = identity.Name,
                DisplayName = identity.DisplayName,
                Description = identity.Description,
                Emphasize = identity.Emphasize,
                Required = identity.Required,
                Checked = check || identity.Required
            };
        }

        public static ScopeViewModel CreateScopeViewModel(this Scope scope, bool check)
        {
            return new ScopeViewModel
            {
                Name = scope.Name,
                DisplayName = scope.DisplayName,
                Description = scope.Description,
                Emphasize = scope.Emphasize,
                Required = scope.Required,
                Checked = check || scope.Required
            };
        }

        public static ScopeViewModel GetOfflineAccessScope(this ScopeViewModel model, bool check)
        {
            model.Name = IdentityServer4.IdentityServerConstants.StandardScopes.OfflineAccess;
            model.DisplayName = ConsentOptions.OfflineAccessDisplayName;
            model.Description = ConsentOptions.OfflineAccessDescription;
            model.Emphasize = true;
            model.Checked = check;

            return model;
        }
    }
}
