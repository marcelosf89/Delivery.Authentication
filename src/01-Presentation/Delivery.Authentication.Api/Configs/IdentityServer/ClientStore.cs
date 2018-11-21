using Delivery.Authentication.Domain.Model;
using IdentityServer4.Models;
using IdentityServer4.Stores;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Delivery.Authentication.Application.Query.IdentityManager.Model;
using Delivery.Authentication.Application.Query;

namespace Delivery.Authentication.Api.Configs.IdentityServer
{
    public class ClientStore : IClientStore
    {
        private readonly IQueryAsync<GetClientByClientIdQueryRequest, IdentityClient> _getClientByClientId;

        public ClientStore(IQueryAsync<GetClientByClientIdQueryRequest, IdentityClient> getClientByClientId)
        {
            _getClientByClientId = getClientByClientId;
        }

        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            return ConvertToClient(await _getClientByClientId.ExecuteAsync(new GetClientByClientIdQueryRequest
            {
                ClientId = clientId
            }));
        }

        private Client ConvertToClient(IdentityClient identityClient)
        {
            return new Client
            {
                ClientId = identityClient.ClientId,
                ClientName = identityClient.ClientDescription,
                AccessTokenLifetime = identityClient.TimeLife,
                AllowedGrantTypes = identityClient.GrantTypes,
                RequireClientSecret = identityClient.RequireClientSecret,
                ClientSecrets = new[] { new Secret((identityClient.ClientSecret ?? "").Sha256()) },
                AllowAccessTokensViaBrowser = identityClient.AllowAccessInBrowser,
                AllowedScopes = identityClient.Scopes,
                RedirectUris = identityClient.RedirectUris,
                PostLogoutRedirectUris = identityClient.PostLogoutRedirectUris,
                AllowedCorsOrigins = identityClient.Authority,
                AllowOfflineAccess = identityClient.AllowOfflineAccess,
                Claims = GetClaims(identityClient.Claims).ToList()
            };
        }

        private IEnumerable<System.Security.Claims.Claim> GetClaims(ICollection<IdentityClaim> claims)
        {
            foreach (IdentityClaim item in claims)
            {
                yield return new System.Security.Claims.Claim(item.Type, item.Value);
            }
        }
    }
}