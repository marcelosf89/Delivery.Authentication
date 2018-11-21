using System;

namespace Delivery.Authentication.Application.Query.UserManager.Models
{
    public class GetUserClaimByUserIdQueryRequest
    {
        public Guid UserId { get; set; }
    }
}
