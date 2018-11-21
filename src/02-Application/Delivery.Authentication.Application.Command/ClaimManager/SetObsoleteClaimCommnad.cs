using Delivery.Authentication.Application.Command.ClaimManager.Models;
using Delivery.Authentication.Application.Command.UserManager.Models;
using Delivery.Authentication.Crosscutting.Exceptions;
using Delivery.Authentication.Crosscutting.Helper;
using Delivery.Authentication.Domain.Model;
using Delivery.Authentication.Infrastructure.Data;
using System.Net;

namespace Delivery.Authentication.Application.Command.ClaimManager
{
    public class SetObsoleteClaimCommnad : ICommand<SetObsoleteClaimCommnadRequest>
    {
        private readonly IClaimData _claimData;

        public SetObsoleteClaimCommnad(IClaimData claimData)
        {
            _claimData = claimData;
        }

        public void Execute(SetObsoleteClaimCommnadRequest setObsoleteClaimCommnadModel)
        {
            _claimData.SetObsolete(setObsoleteClaimCommnadModel.Code);
        }
    }
}
