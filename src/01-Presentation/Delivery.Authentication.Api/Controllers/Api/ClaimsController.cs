using Delivery.Authentication.Application.Command;
using Delivery.Authentication.Application.Command.ClaimManager.Models;
using Delivery.Authentication.Crosscutting.Request.ClaimManagement;
using Delivery.Authentication.Domain.Model;
using Microsoft.AspNetCore.Mvc;
using Delivery.Authentication.Crosscutting.Response;
using System.Net;
using Delivery.Authentication.Application.Query;
using Delivery.Authentication.Application.Query.ClaimManager.Model;

namespace Delivery.Authentication.Api.Controllers
{
    [Route("claims")]
    public class ClaimsController : Controller
    {
        private readonly IQuery<GetClaimByCodeQueryRequest, Claim> _getClaimByCodeQuery;
        private readonly ICommand<SaveClaimRequest> _saveClaimCommand;
        private readonly ICommand<UpdateClaimRequest> _updateClaimCommand;
        private readonly ICommand<SetObsoleteClaimCommnadRequest> _setObsoleteClaimCommand;

        public ClaimsController
            (IQuery<GetClaimByCodeQueryRequest, Claim> getClaimByCodeQuery,
                                ICommand<SaveClaimRequest> saveClaimCommand,
                                ICommand<UpdateClaimRequest> updateClaimCommand,
                                ICommand<SetObsoleteClaimCommnadRequest> setObsoleteClaimCommand)
        {
            _getClaimByCodeQuery = getClaimByCodeQuery;
            _saveClaimCommand = saveClaimCommand;
            _updateClaimCommand = updateClaimCommand;
            _setObsoleteClaimCommand = setObsoleteClaimCommand;
        }

        /// <summary>
        /// Get Claim by Code
        /// </summary>
        /// <param name="code">User Id</param>
        /// <returns>Claim</returns>
        /// <response code="400">The claim does not exist</response>
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
        [HttpGet("{code}")]
        public IActionResult Get(string code)
        {
            Claim claim = _getClaimByCodeQuery.Execute(new GetClaimByCodeQueryRequest
            {
                Code = code
            });

            return Ok(claim);
        }

        /// <summary>
        /// create a new claim
        /// </summary>
        /// <param name="claim">SaveClaimRequest</param>
        /// <returns></returns>
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
        [HttpPost()]
        public IActionResult Save([FromBody] SaveClaimRequest claim)
        {
            _saveClaimCommand.Execute(claim);

            return NoContent();
        }

        /// <summary>
        /// Update claim
        /// </summary>
        /// <param name="claim">UpdateClaimRequest</param>
        /// <returns></returns>
        /// <response code="400">The claim code does not exist</response>
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
        [HttpPut()]
        public IActionResult Update([FromBody] UpdateClaimRequest claim)
        {
            _updateClaimCommand.Execute(claim);

            return NoContent();
        }

        /// <summary>
        /// Set claim obsolete
        /// </summary>
        /// <param name="code">Claim code</param>
        /// <returns></returns>
        /// <response code="400">The claim code does not exist</response>
        [ProducesResponseType(typeof(User), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
        [HttpPut("{code}/obsolete")]
        public IActionResult SetObsolete(string  code)
        {
            _setObsoleteClaimCommand.Execute(new SetObsoleteClaimCommnadRequest
            {
                Code = code
            });

            return NoContent();
        }
    }
}