using System;
using System.Collections.Generic;
using System.Text;

namespace Delivery.Authentication.Application.Query
{
    public interface IQuery<TRequest,TResponse>
    {

        TResponse Execute(TRequest request);
    }
}
