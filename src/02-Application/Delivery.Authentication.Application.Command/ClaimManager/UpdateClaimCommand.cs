using Delivery.Authentication.Crosscutting.Exceptions;
using Delivery.Authentication.Crosscutting.Request.ClaimManagement;
using Delivery.Authentication.Domain.Model;
using Delivery.Authentication.Infrastructure.Data;
using System.Net;

namespace Delivery.Authentication.Application.Command.ClaimManager
{
    public class UpdateClaimCommand : ICommand<UpdateClaimRequest>
    {
        private readonly IClaimData _claimData;

        public UpdateClaimCommand(IClaimData claimData)
        {
            _claimData = claimData;
        }

        public void Execute(UpdateClaimRequest claimRequest)
        {
            Claim claim = _claimData.GetClaim(claimRequest.Code);

            if (claim is null) throw new ResponseException(HttpStatusCode.BadRequest, $"The claim id does not exist");

            claim.Description = claimRequest.Description;

            _claimData.Update(claim);
        }
    }
}
