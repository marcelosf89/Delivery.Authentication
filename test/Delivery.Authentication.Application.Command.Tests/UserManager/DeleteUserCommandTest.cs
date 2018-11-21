using Delivery.Authentication.Application.Command.UserManager;
using Delivery.Authentication.Application.Command.UserManager.Models;
using Delivery.Authentication.Crosscutting.Exceptions;
using Delivery.Authentication.Crosscutting.Helper;
using Delivery.Authentication.Domain.Model;
using Delivery.Authentication.Infrastructure.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace Delivery.Authentication.Application.Command.Tests.UserManager
{
    [TestClass]
    public class DeleteUserCommandTest
    {
        private Mock<IUserData>  _userdataMock;

        [TestInitialize]
        public void Init()
        {
            _userdataMock = new Mock<IUserData>();
        }

        [TestMethod]
        [ExpectedException(typeof(ResponseException), "The user id does not exist")]
        public void DeleteUserCommand_InvalidId_ThrowResponseException400()
        {
            //Arrenge
            Guid id = Guid.NewGuid();

            _userdataMock.Setup(p => p.GetUser(It.IsAny<Guid>()));

            //Act
            new DeleteUserCommand(_userdataMock.Object).Execute(new DeleteUserCommnadRequest
            {
                Id = id
            });
        }

        [TestMethod]
        public void DeleteUserCommand_Ok()
        {
            //Arrenge
            Guid id = Guid.NewGuid();

            _userdataMock.Setup(p => p.GetUser(It.IsIn(id)))
                .Returns( new User
                {
                    Id = id,
                    Email = "test@test.com",
                    Username = "test"                    
                });

            //Act
            new DeleteUserCommand(_userdataMock.Object).Execute(new DeleteUserCommnadRequest
            {
                Id = id
            });

            //Assert
            _userdataMock.Verify(p => p.Delete(It.IsIn(id)), Times.Once);
        }
    }
}
