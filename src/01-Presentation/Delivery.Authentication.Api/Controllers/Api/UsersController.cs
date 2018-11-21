using Delivery.Authentication.Application.Command;
using Delivery.Authentication.Application.Command.UserManager.Models;
using Delivery.Authentication.Application.Query.UserManager.Models;
using Delivery.Authentication.Crosscutting.Request.UserManagement;
using Delivery.Authentication.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using Delivery.Authentication.Crosscutting.Response;
using System.Net;
using Delivery.Authentication.Application.Query;

namespace Delivery.Authentication.Api.Controllers
{
    [Route("users")]
    public class UsersController : Controller
    {
        private readonly IQuery<GetUserByIdQueryRequest, User> _getUserByIdQuery;
        private readonly ICommand<SaveUserRequest> _saveUserCommand;
        private readonly ICommand<UpdateUserRequest> _updateUserCommand;
        private readonly ICommand<DeleteUserCommnadRequest> _deleteUserCommand;
        private readonly ICommand<AddClaimUserCommnadRequest> _addClaimUserCommand;
        private readonly ICommand<RemoveClaimUserCommnadRequest> _removeClaimUserCommand;

        public UsersController(IQuery<GetUserByIdQueryRequest, User> getUserByIdQuery,
                                ICommand<SaveUserRequest> saveUserCommand,
                                ICommand<UpdateUserRequest> updateUserCommand,
                                ICommand<DeleteUserCommnadRequest> deleteUserCommand,
                                ICommand<AddClaimUserCommnadRequest> addClaimUserCommand,
                                ICommand<RemoveClaimUserCommnadRequest> removeClaimUserCommand)

        {
            _getUserByIdQuery = getUserByIdQuery;
            _saveUserCommand = saveUserCommand;
            _updateUserCommand = updateUserCommand;
            _deleteUserCommand = deleteUserCommand;
            _addClaimUserCommand = addClaimUserCommand;
            _removeClaimUserCommand = removeClaimUserCommand;
        }


        /// <summary>
        /// Get User by Code
        /// </summary>
        /// <param name="id">User Id</param>
        /// <returns>User</returns>
        /// <response code="400">The user does not exist</response>
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
        [HttpGet("{id}")]
        public IActionResult Get(Guid id)
        {
            User user = _getUserByIdQuery.Execute(new GetUserByIdQueryRequest
            {
                Id = id
            });

            return Ok(user);
        }


        /// <summary>
        /// Create the new user
        /// </summary>
        /// <param name="user">SaveUserRequest</param>
        /// <returns></returns>
        /// <response code="400">The username already exists / The email already exists</response>
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
        [HttpPost()]
        public IActionResult Save([FromBody] SaveUserRequest user)
        {
            _saveUserCommand.Execute(user);

            return NoContent();
        }

        /// <summary>
        /// Update the new user
        /// </summary>
        /// <param name="user">UpdateUserRequest</param>
        /// <returns></returns>
        /// <response code="400">The user id does not exist</response>
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
        [HttpPut()]
        public IActionResult Update([FromBody] UpdateUserRequest user)
        {
            _updateUserCommand.Execute(user);

            return NoContent();
        }

        /// <summary>
        /// Add new user's claims
        /// </summary>
        /// <param name="request">AddClaimUserRequest</param>
        /// <returns></returns>
        /// <response code="400">The username does not exist</response>
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
        [HttpPut("addclaims")]
        public IActionResult AddClaim([FromBody] AddClaimUserRequest request)
        {
            _addClaimUserCommand.Execute(new AddClaimUserCommnadRequest
            {
                Claims = request.Claims,
                Username = request.Username
            });

            return NoContent();
        }

        /// <summary>
        /// Remove user's claims 
        /// </summary>
        /// <param name="request">RemoveClaimUserRequest</param>
        /// <returns></returns>
        /// <response code="400">The username does not exist</response>
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
        [HttpPut("removeclaims")]
        public IActionResult RemoveClaim([FromBody] RemoveClaimUserRequest request)
        {
            _removeClaimUserCommand.Execute(new RemoveClaimUserCommnadRequest
            {
                Claims = request.Claims,
                Username = request.Username
            });

            return NoContent();
        }

        /// <summary>
        /// Delete the new user
        /// </summary>
        /// <param name="id">user id</param>
        /// <returns></returns>
        /// <response code="400">The user id does not exist</response>
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            _deleteUserCommand.Execute(new DeleteUserCommnadRequest
            {
                Id = id
            });

            return NoContent();
        }
    }
}