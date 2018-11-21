using Delivery.Authentication.Domain.Model;
using System.Collections.Generic;

namespace Delivery.Authentication.Infrastructure.Data
{
    public interface IClaimSearchData
    {
        IEnumerable<Claim> GetAllClaims();

        IEnumerable<Claim> GetAllClaims(string name);

        IEnumerable<Claim> GetAllClaimsNextPage(string token);

        void SetClaim(Claim claim);
    }
}
