using Delivery.Authentication.Application.Query.IdentityManager.Model;
using Delivery.Authentication.Domain.Model;
using Delivery.Authentication.Infrastructure.Data.IdentityManager;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Delivery.Authentication.Application.Query.IdentityManager
{
    public class GetIdentityApiByApiScopesQuery : IQueryAsync<GetIdentityApiByApiScopesQueryRequest, IEnumerable<IdentityApi>>
    {
        private readonly IIdentityApiData _identityApiData;

        public GetIdentityApiByApiScopesQuery(IIdentityApiData identityApiData)
        {
            _identityApiData = identityApiData;
        }

        public async Task<IEnumerable<IdentityApi>> ExecuteAsync(GetIdentityApiByApiScopesQueryRequest request)
        {
            if (request.ApiScope is null || request.ApiScope.Length <= 0) return null;

            IEnumerable<IdentityApi> identityApiLIst = await _identityApiData.GetIdentityApiByApiScopesAsync(request.ApiScope);

            return identityApiLIst;
        }
    }
}
