using Delivery.Authentication.Crosscutting.Exceptions;
using Delivery.Authentication.Crosscutting.Request.ClaimManagement;
using Delivery.Authentication.Domain.Model;
using Delivery.Authentication.Infrastructure.Data;
using System.Net;

namespace Delivery.Authentication.Application.Command.ClaimManager
{
    public class SaveClaimCommand : ICommand<SaveClaimRequest>
    {
        private readonly IClaimData _claimData;

        public SaveClaimCommand(IClaimData claimdata)
        {
            _claimData = claimdata;
        }

        public void Execute(SaveClaimRequest claimRequest)
        {
            claimRequest.Code = claimRequest.Code.ToLower();
            claimRequest.Description = claimRequest.Description;

            CheckIfClaimExist(claimRequest.Code);

            Claim claim = new Claim
            {
                Code = claimRequest.Code,
                Description = claimRequest.Description,
                IsObsolete = false
            };

            _claimData.Save(claim);
        }

        private void CheckIfClaimExist(string code)
        {
            if (_claimData.HasClaim(code))
            {
                throw new ResponseException(HttpStatusCode.BadRequest, "The code already exists");
            }
        }
    }
}
