using System;
using Delivery.Authentication.Application.Command.UserManager;
using Delivery.Authentication.Application.Command.UserManager.Models;
using Delivery.Authentication.Crosscutting.Exceptions;
using Delivery.Authentication.Domain.Model;
using Delivery.Authentication.Infrastructure.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Delivery.Authentication.Application.Command.Tests.UserManager
{
    [TestClass]
    public class RemoveClaimUserCommnadTest
    {
        private Mock<IUserData> _userdataMock;

        [TestInitialize]
        public void Init()
        {
            _userdataMock = new Mock<IUserData>();
        }

        [TestMethod]
        [ExpectedException(typeof(ResponseException), "The username does not exist")]
        public void RemoveClaimUserCommnad_ThrowUsernameDoesntExist()
        {
            //Arrenge
            _userdataMock.Setup(p => p.GetUserByUsername(It.IsAny<string>()));

            //Act
            new RemoveClaimUserCommnad(_userdataMock.Object).Execute(new RemoveClaimUserCommnadRequest
            {
                Username = "new",
                Claims = new string[] { }
            });
        }

        [TestMethod]
        public void RemoveClaimUserCommnad_Ok()
        {
            //Arrenge
            User user = new User
            {
                Id = Guid.NewGuid(),
                Email = "test@test.com",
                Username = "test",
                FirstName = "FirstName",
                LastName = "LastName",
                Phone = "Phone"
            };

            _userdataMock.Setup(p => p.GetUserByUsername(It.IsIn(user.Username)))
                .Returns(user);

            //Act
            new RemoveClaimUserCommnad(_userdataMock.Object).Execute(new RemoveClaimUserCommnadRequest
            {
                Username = user.Username,
                Claims = new string[] { "aa" }
            });

            //Assert
            _userdataMock.Verify(p => p.UpdeteClaims(It.Is((Guid id) =>
                id == user.Id
            )));
        }
    }
}