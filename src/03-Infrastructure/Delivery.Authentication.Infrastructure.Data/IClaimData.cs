using Delivery.Authentication.Domain.Model;
using System;

namespace Delivery.Authentication.Infrastructure.Data
{
    public interface IClaimData
    {
        Claim GetClaim(string code);

        void Update(Claim user);

        void Save(Claim user);

        void SetObsolete(string code);

        bool HasClaim(string code);
    }
}
