using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Delivery.Authentication.Application.Query
{
    public interface IQueryAsync<TRequest,TResponse>
    {

        Task<TResponse> ExecuteAsync(TRequest request);
    }
}
