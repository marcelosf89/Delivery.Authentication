using Delivery.Authentication.Application.Query;
using Delivery.Authentication.Application.Query.IdentityManager;
using Delivery.Authentication.Application.Query.IdentityManager.Model;
using Delivery.Authentication.Domain.Model;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Delivery.Authentication.Api.Configs.IdentityServer
{
    public class ResourceStore : IResourceStore
    {
        private readonly IQueryAsync<GetIdentityApiByApiScopesQueryRequest, IEnumerable<IdentityApi>> _getIdentityApiByApiScopesQuery;

        public ResourceStore(IQueryAsync<GetIdentityApiByApiScopesQueryRequest, IEnumerable<IdentityApi>> getIdentityApiByApiScopesQuery)
        {
            _getIdentityApiByApiScopesQuery = getIdentityApiByApiScopesQuery;
        }

        private IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }

        public async Task<ApiResource> FindApiResourceAsync(string name)
        {
            IEnumerable<ApiResource> result = await GetApiResourceList(name);

            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<ApiResource>> FindApiResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            IEnumerable<ApiResource> result = await GetApiResourceList(scopeNames.ToArray());

            return result;
        }

        public Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeAsync(IEnumerable<string> scopeNames)
        {
            var identityResources = GetIdentityResources();

            return Task.FromResult(identityResources);
        }

        public Task<Resources> GetAllResourcesAsync()
        {
            var resources = new Resources();

            return Task.FromResult(resources);
        }

        private async Task<IEnumerable<ApiResource>> GetApiResourceList(params string[] apiScopes)
        {
            IEnumerable<IdentityApi> response = await _getIdentityApiByApiScopesQuery
                .ExecuteAsync(new GetIdentityApiByApiScopesQueryRequest
                {
                    ApiScope = apiScopes
                });
            List<ApiResource> resources = new List<ApiResource>();

            foreach (IdentityApi apiResource in response)
            {
                ApiResource result = new ApiResource(apiResource.Code, apiResource.Description, apiResource.Claims);

                resources.Add(result);
            }

            return resources;
        }
    }
}
