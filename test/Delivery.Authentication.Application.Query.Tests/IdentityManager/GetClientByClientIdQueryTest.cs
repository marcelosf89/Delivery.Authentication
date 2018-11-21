using Delivery.Authentication.Application.Query.IdentityManager;
using Delivery.Authentication.Application.Query.IdentityManager.Model;
using Delivery.Authentication.Crosscutting.Exceptions;
using Delivery.Authentication.Domain.Model;
using Delivery.Authentication.Infrastructure.Data;
using Delivery.Authentication.Infrastructure.Data.IdentityManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Delivery.Authentication.Application.Query.Tests.IdentityManager
{
    [TestClass]
    public class GetClientByClientIdQueryTest
    {
        private Mock<IIdentityClientData> _identityClientDataMock;

        [TestInitialize]
        public void Init()
        {
            _identityClientDataMock = new Mock<IIdentityClientData>();
        }


        [TestMethod]
        [ExpectedException(typeof(ResponseException), "The identity client not exists")]
        public async Task GetClientByClientIdQuery_InvalidClientId_ThrowResponseException400()
        {
            //Arrenge
            string clientId = "testErr";
            IdentityClient client = null;

            //Act
            _identityClientDataMock.Setup(p => p.GetIdentityClientByClientIdAsync(It.IsIn(clientId)))
                .ReturnsAsync(client);

            //Assert
            await new GetClientByClientIdQuery(_identityClientDataMock.Object).ExecuteAsync(
                new GetClientByClientIdQueryRequest { ClientId = clientId });
        }

        [TestMethod]
        public async Task GetClientByClientIdQuery_Ok()
        {
            //Arrenge
            string clientId = "testSuccess";

            _identityClientDataMock.Setup(p => p.GetIdentityClientByClientIdAsync(It.IsIn(clientId)))
                .ReturnsAsync(new IdentityClient
                {
                    ClientSecret = clientId,
                    ClientId = clientId,
                });

            //Act
            IdentityClient client =  await new GetClientByClientIdQuery(_identityClientDataMock.Object).ExecuteAsync(
                new GetClientByClientIdQueryRequest
                {
                    ClientId = clientId
                });

            //Assert
            Assert.IsNotNull(client);
            Assert.IsTrue(client.ClientSecret.Equals(clientId));
            Assert.IsTrue(client.ClientId.Equals(clientId));
        }
    }
}
