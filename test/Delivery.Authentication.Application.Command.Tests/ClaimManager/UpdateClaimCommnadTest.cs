using System;
using System.Text;
using Delivery.Authentication.Application.Command.ClaimManager;
using Delivery.Authentication.Crosscutting.Exceptions;
using Delivery.Authentication.Crosscutting.Helper;
using Delivery.Authentication.Crosscutting.Request.ClaimManagement;
using Delivery.Authentication.Domain.Model;
using Delivery.Authentication.Infrastructure.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Delivery.Authentication.Application.Command.Tests.ClaimManager
{
    [TestClass]
    public class UpdateClaimCommnadTest
    {
        private Mock<IClaimData> _claimdataMock;
        private Mock<IPasswordHash> _passwordHashMock;

        [TestInitialize]
        public void Init()
        {
            _claimdataMock = new Mock<IClaimData>();
            _passwordHashMock = new Mock<IPasswordHash>();
        }

        [TestMethod]
        [ExpectedException(typeof(ResponseException), "The claim id does not exist")]
        public void UpdateClaimCommand_InvalidId_ThrowResponseException400()
        {
            //Arrenge
            string claim = "TEST";

            _claimdataMock.Setup(p => p.GetClaim(It.IsAny<string>()));

            //Act
            new UpdateClaimCommand(_claimdataMock.Object).Execute(new UpdateClaimRequest
            {
                Code = claim
            });
        }

        [TestMethod]
        public void UpdateClaimCommand_Ok()
        {
            //Arrenge
            Claim claim = new Claim
            {
                Code = "Test",
                Description = "test2",
                IsObsolete = true
            };

            Claim claimExcpectation = new Claim
            {
                Code = "Test",
                Description = "test"
            };

            _claimdataMock.Setup(p => p.GetClaim(It.IsIn(claim.Code)))
                .Returns(claim);

            //Act
            new UpdateClaimCommand(_claimdataMock.Object).Execute(new UpdateClaimRequest
            {
                Code = claimExcpectation.Code,
                Description = claimExcpectation.Description
            });

            //Assert
            _claimdataMock.Verify(p => p.Update(It.Is((Claim c) =>
                c.Code == claimExcpectation.Code &&
                c.Description == claimExcpectation.Description &&
                c.IsObsolete
            )));
        }
    }
}
