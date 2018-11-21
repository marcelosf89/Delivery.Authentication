using Delivery.Authentication.Application.Query.ClaimManager;
using Delivery.Authentication.Application.Query.ClaimManager.Model;
using Delivery.Authentication.Crosscutting.Exceptions;
using Delivery.Authentication.Domain.Model;
using Delivery.Authentication.Infrastructure.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Delivery.Authentication.Application.Command.Tests.UserManager
{
    [TestClass]
    public class GetClaimByCodeQueryTest
    {
        private Mock<IClaimData> _claimdataMock;

        [TestInitialize]
        public void Init()
        {
            _claimdataMock = new Mock<IClaimData>();
        }

        [TestMethod]
        [ExpectedException(typeof(ResponseException), "The claim does not exist")]
        public void GetClaimByCodeQuery_InvalidId_ThrowResponseException400()
        {
            //Arrenge
            string code = "test";

            _claimdataMock.Setup(p => p.GetClaim(It.IsAny<string>()));

            //Act
            new GetClaimByCodeQuery(_claimdataMock.Object).Execute(new GetClaimByCodeQueryRequest
            {
                Code = code
            });
        }

        [TestMethod]
        public void GetClaimByCodeQuery_Ok()
        {
            //Arrenge
            string code = "test";

            _claimdataMock.Setup(p => p.GetClaim(It.IsIn(code)))
                .Returns(new Claim
                {
                    Code = code,
                    Description = "test",
                    IsObsolete = false
                });

            Claim claimExpectation = new Claim
            {
                Code = code,
                Description = "test",
                IsObsolete = false
            };

            //Act
            Claim claim = new GetClaimByCodeQuery(_claimdataMock.Object).Execute(new GetClaimByCodeQueryRequest
            {
                Code = code
            });

            //Assert
            _claimdataMock.Verify(p => p.GetClaim(It.IsIn(code)), Times.Once);

            Assert.IsTrue(claimExpectation.Code == claim.Code);
            Assert.IsTrue(claimExpectation.Description == claim.Description);
            Assert.IsTrue(claimExpectation.IsObsolete == claim.IsObsolete);
        }
    }
}
