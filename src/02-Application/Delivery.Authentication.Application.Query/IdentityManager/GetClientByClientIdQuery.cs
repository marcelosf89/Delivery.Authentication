using Delivery.Authentication.Application.Query.IdentityManager.Model;
using Delivery.Authentication.Crosscutting.Exceptions;
using Delivery.Authentication.Domain.Model;
using Delivery.Authentication.Infrastructure.Data.IdentityManager;
using System.Threading.Tasks;

namespace Delivery.Authentication.Application.Query.IdentityManager
{
    public class GetClientByClientIdQuery : IQueryAsync<GetClientByClientIdQueryRequest, IdentityClient>
    {
        private readonly IIdentityClientData _identityClientData;

        public GetClientByClientIdQuery(IIdentityClientData identityClientData)
        {
            _identityClientData = identityClientData;
        }

        public async Task<IdentityClient> ExecuteAsync(GetClientByClientIdQueryRequest request)
        {
            IdentityClient identityClient = await _identityClientData.GetIdentityClientByClientIdAsync(request.ClientId);

            if (identityClient == null)
            {
                throw new ResponseException(System.Net.HttpStatusCode.BadRequest, $"The identity client not exists");
            }

            return identityClient;
        }
    }
}
