using Delivery.Authentication.Application.Command.ClaimManager;
using Delivery.Authentication.Application.Command.ClaimManager.Models;
using Delivery.Authentication.Crosscutting.Exceptions;
using Delivery.Authentication.Domain.Model;
using Delivery.Authentication.Infrastructure.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Delivery.Authentication.Application.Command.Tests.ClaimManager
{
    [TestClass]
    public class SetObsoleteClaimCommnadTest
    {
        private Mock<IClaimData> _claimdataMock;

        [TestInitialize]
        public void Init()
        {
            _claimdataMock = new Mock<IClaimData>();
        }

        [TestMethod]
        public void DeleteClaimCommand_Ok()
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

            //Act
            new SetObsoleteClaimCommnad(_claimdataMock.Object).Execute(new SetObsoleteClaimCommnadRequest
            {
                Code = code
            });

            //Assert
            _claimdataMock.Verify(p => p.SetObsolete(It.IsIn(code)), Times.Once);
        }
    }
}
