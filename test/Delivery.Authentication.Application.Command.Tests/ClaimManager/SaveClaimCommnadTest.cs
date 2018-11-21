using Delivery.Authentication.Application.Command.ClaimManager;
using Delivery.Authentication.Crosscutting.Exceptions;
using Delivery.Authentication.Crosscutting.Request.ClaimManagement;
using Delivery.Authentication.Domain.Model;
using Delivery.Authentication.Infrastructure.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Delivery.Authentication.Application.Command.Tests.ClaimManager
{
    [TestClass]
    public class SaveClaimCommnadTest
    {
        private Mock<IClaimData> _claimdataMock;

        [TestInitialize]
        public void Init()
        {
            _claimdataMock = new Mock<IClaimData>();
        }

        [TestMethod]
        [ExpectedException(typeof(ResponseException), "The code already exists")]
        public void SaveClaimCommnad_ThrowUsernameExists()
        {
            //Arrenge
            _claimdataMock.Setup(p => p.HasClaim(It.IsIn("test")))
                                .Returns(true);

            //Act
            new SaveClaimCommand(_claimdataMock.Object).Execute(new SaveClaimRequest
            {
                Code = "test",
                Description = "test"
            });
        }

        [TestMethod]
        public void SaveClaimCommnad_Ok()
        {
            //Arrenge
            Claim claim = new Claim
            {
                Code = "TEST",
                Description = "test"
            };

            _claimdataMock.Setup(p => p.GetClaim(It.IsIn(claim.Code)));

            //Act
            new SaveClaimCommand(_claimdataMock.Object).Execute(new SaveClaimRequest
            {
                Code = claim.Code,
                Description = claim.Description
            });

            _claimdataMock.Verify(p => p.Save(It.Is((Claim c) =>
                                                c.Code == claim.Code.ToLower() &&
                                                c.Description == claim.Description &&
                                                !c.IsObsolete
                                                )));
        }
    }
}
