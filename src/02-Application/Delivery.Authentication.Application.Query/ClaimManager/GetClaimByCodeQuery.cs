using Delivery.Authentication.Application.Query.ClaimManager.Model;
using Delivery.Authentication.Crosscutting.Exceptions;
using Delivery.Authentication.Domain.Model;
using Delivery.Authentication.Infrastructure.Data;

namespace Delivery.Authentication.Application.Query.ClaimManager
{
    public class GetClaimByCodeQuery : IQuery<GetClaimByCodeQueryRequest, Claim>
    {
        private readonly IClaimData _claimData;

        public GetClaimByCodeQuery(IClaimData claimData)
        {
            _claimData = claimData;
        }

        public Claim Execute(GetClaimByCodeQueryRequest request)
        {
            Claim claim = _claimData.GetClaim(request.Code);

            if (claim == null)
            {
                throw new ResponseException(System.Net.HttpStatusCode.BadRequest, $"The claim does not exist");
            }

            return claim;
        }
    }
}
