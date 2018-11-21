using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Delivery.Authentication.Domain.Model;

namespace Delivery.Authentication.Infrastructure.Data.IdentityManager
{
    public interface IIdentityApiData
    {
        Task<IEnumerable<IdentityApi>> GetIdentityApiByApiScopesAsync(string[] apiScope);
    }
}
