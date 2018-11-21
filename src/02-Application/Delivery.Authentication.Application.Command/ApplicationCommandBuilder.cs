using Delivery.Authentication.Application.Command.ClaimManager;
using Delivery.Authentication.Application.Command.ClaimManager.Models;
using Delivery.Authentication.Application.Command.UserManager;
using Delivery.Authentication.Application.Command.UserManager.Models;
using Delivery.Authentication.Crosscutting.Request.ClaimManagement;
using Delivery.Authentication.Crosscutting.Request.UserManagement;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Delivery.Authentication.Application.Command
{
    [ExcludeFromCodeCoverage]
    public static class ApplicationCommandBuilder
    {
        public static void AddApplicationCommand(this IServiceCollection services)
        {
            services.AddScoped<ICommand<SaveUserRequest>, SaveUserCommand>();
            services.AddScoped<ICommand<UpdateUserRequest>, UpdateUserCommand>();
            services.AddScoped<ICommand<DeleteUserCommnadRequest>, DeleteUserCommand>();
            services.AddScoped<ICommand<AddClaimUserCommnadRequest>, AddClaimUserCommnad>();
            services.AddScoped<ICommand<RemoveClaimUserCommnadRequest>, RemoveClaimUserCommnad>();

            services.AddScoped<ICommand<SaveClaimRequest>, SaveClaimCommand>();
            services.AddScoped<ICommand<UpdateClaimRequest>, UpdateClaimCommand>();
            services.AddScoped<ICommand<SetObsoleteClaimCommnadRequest>, SetObsoleteClaimCommnad>();
        }
    }
}
