using Delivery.Authentication.Domain.Model;
using System.Threading.Tasks;

namespace Delivery.Authentication.Infrastructure.Data.IdentityManager
{
    public interface IIdentityClientData
    {
        Task<IdentityClient> GetIdentityClientByClientIdAsync(string clientId);
    }
}
