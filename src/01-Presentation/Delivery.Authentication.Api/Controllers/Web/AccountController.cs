using Delivery.Authentication.Api.Models;
using IdentityServer4.Events;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Delivery.Authentication.Api.Controllers.Web;
using Microsoft.AspNetCore.Http;
using Delivery.Authentication.Application.Query.UserManager;
using System.Collections.Generic;
using System.Linq;
using Delivery.Authentication.Crosscutting.Response.UserManagement;
using Delivery.Authentication.Application.Query;
using Delivery.Authentication.Application.Query.UserManager.Models;

namespace Delivery.Authentication.Api.Controllers
{
    public class AccountController : Controller
    {
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IEventService _events;
        private readonly IClientStore _clientStore;
        private readonly IQuery<GetUserAuthByUsernameAndPasswordQueryRequest, UserAuthInfoResponse> _getUserByUsernameAndPasswordQuery;

        public AccountController(
            IIdentityServerInteractionService interaction,
            IEventService events,
            IAuthenticationSchemeProvider schemeProvider,
            IClientStore clientStore,
            IQuery<GetUserAuthByUsernameAndPasswordQueryRequest, UserAuthInfoResponse> getUserByUsernameAndPasswordQuery)
        {
            _interaction = interaction;
            _events = events;
            _schemeProvider = schemeProvider;
            _clientStore = clientStore;
            _getUserByUsernameAndPasswordQuery = getUserByUsernameAndPasswordQuery;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginViewModel
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Redirect("~/");
            }

            var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);
            if (context == null)
            {
                return View(model);
            }

            UserAuthInfoResponse user = _getUserByUsernameAndPasswordQuery.Execute(
                new GetUserAuthByUsernameAndPasswordQueryRequest
                {
                    Username = model.Username,
                    Password = model.Password
                });

            if (user is null)
            {
                await _events.RaiseAsync(new UserLoginFailureEvent(model.Username, "invalid credentials"));
                ModelState.AddModelError("", AccountOptions.InvalidCredentialsErrorMessage);

                return View(model);
            }

            await _events.RaiseAsync(new UserLoginSuccessEvent(user.Username, user.Id.ToString(), user.Username));

            HashSet<System.Security.Claims.Claim> userClaimsList = new HashSet<System.Security.Claims.Claim>();
            userClaimsList.Add(new System.Security.Claims.Claim("firstname", user.FirstName));
            userClaimsList.Add(new System.Security.Claims.Claim("lastname", user.LastName));
            userClaimsList.Add(new System.Security.Claims.Claim("username", user.Username));

            if (!(user.Claims is null))
            {
                foreach (string role in user.Claims)
                {
                    userClaimsList.Add(new System.Security.Claims.Claim("role", role));
                }
            }

            await HttpContext.SignInAsync(user.Id.ToString(), user.Username, userClaimsList.ToArray());

            if (await _clientStore.IsPkceClientAsync(context.ClientId))
            {
                return View("Redirect", model.ReturnUrl);
            }
            else if (Url.IsLocalUrl(model.ReturnUrl))
            {
                return Redirect(model.ReturnUrl);
            }
            else if (string.IsNullOrEmpty(model.ReturnUrl))
            {
                return Redirect("~/");
            }

            throw new Exception("invalid return URL");
        }
    }
}