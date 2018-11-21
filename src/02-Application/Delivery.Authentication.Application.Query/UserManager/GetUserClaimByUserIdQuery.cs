using Delivery.Authentication.Application.Query.UserManager.Models;
using Delivery.Authentication.Infrastructure.Data;
using System;
using System.Collections.Generic;

namespace Delivery.Authentication.Application.Query.UserManager
{
    public class GetUserClaimByUserIdQuery : IQuery<GetUserClaimByUserIdQueryRequest, IEnumerable<string>>
    {
        private readonly IUserData _userdata;

        public GetUserClaimByUserIdQuery(IUserData userdata)
        {
            _userdata = userdata;
        }

        public IEnumerable<string> Execute(GetUserClaimByUserIdQueryRequest request)
        {
            IEnumerable<string> listUserClaims = _userdata.GetClaimsByUserId(request.UserId);

            return listUserClaims;
        }
    }
}
