using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.Extensions.DependencyInjection;

namespace Delivery.Authentication.Api.Configs.IdentityServer
{
    public static class IdentityServerBuilder
    {

        public static void AddIdentityConfiguration(this IServiceCollection services)
        {
            services.AddSingleton<IClientStore, ClientStore>();
            services.AddSingleton<IResourceStore, ResourceStore>();
            services.AddSingleton<IProfileService, ProfileService>();

            IIdentityServerBuilder identity = services.AddIdentityServer();
#if DEBUG
            identity.AddDeveloperSigningCredential();
#endif

        }
    }
}
