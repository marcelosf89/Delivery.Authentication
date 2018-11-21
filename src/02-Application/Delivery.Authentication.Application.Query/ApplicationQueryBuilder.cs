using Delivery.Authentication.Application.Query.ClaimManager;
using Delivery.Authentication.Application.Query.ClaimManager.Model;
using Delivery.Authentication.Application.Query.IdentityManager;
using Delivery.Authentication.Application.Query.IdentityManager.Model;
using Delivery.Authentication.Application.Query.UserManager;
using Delivery.Authentication.Application.Query.UserManager.Models;
using Delivery.Authentication.Crosscutting.Response.UserManagement;
using Delivery.Authentication.Domain.Model;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Delivery.Authentication.Application.Query
{
    [ExcludeFromCodeCoverage]
    public static class ApplicationQueryBuilder
    {
        public static void AddApplicationQuery(this IServiceCollection services)
        {
            services.AddScoped<IQuery<GetUserByIdQueryRequest, User>, GetUserByIdQuery>();
            services.AddSingleton<IQuery<GetUserAuthByUsernameAndPasswordQueryRequest, UserAuthInfoResponse>, GetUserAuthByUsernameAndPasswordQuery>();
            services.AddSingleton<IQuery<GetUserClaimByUserIdQueryRequest, IEnumerable<string>>, GetUserClaimByUserIdQuery>();

            services.AddScoped<IQuery<GetClaimByCodeQueryRequest, Claim>, GetClaimByCodeQuery>();

            services.AddSingleton<IQueryAsync<GetClientByClientIdQueryRequest, IdentityClient>, GetClientByClientIdQuery>();

            services.AddSingleton<IQueryAsync<GetIdentityApiByApiScopesQueryRequest, IEnumerable<IdentityApi>>, GetIdentityApiByApiScopesQuery>();
        }
    }
}
