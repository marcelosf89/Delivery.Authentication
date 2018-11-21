using System;
using Delivery.Authentication.Application.Query.UserManager;
using Delivery.Authentication.Application.Query.UserManager.Models;
using Delivery.Authentication.Crosscutting.Exceptions;
using Delivery.Authentication.Domain.Model;
using Delivery.Authentication.Infrastructure.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Delivery.Authentication.Application.Command.Tests.UserManager
{
    [TestClass]
    public class GetUserByIdQueryTest
    {
        private Mock<IUserData> _userdataMock;

        [TestInitialize]
        public void Init()
        {
            _userdataMock = new Mock<IUserData>();
        }

        [TestMethod]
        [ExpectedException(typeof(ResponseException), "The user does not exist")]
        public void GetUserByIdQuery_InvalidId_ThrowResponseException400()
        {
            //Arrenge
            Guid id = Guid.NewGuid();

            _userdataMock.Setup(p => p.GetUser(It.IsAny<Guid>()));

            //Act
            new GetUserByIdQuery(_userdataMock.Object).Execute(new GetUserByIdQueryRequest
            {
                Id = id
            });
        }

        [TestMethod]
        public void GetUserByIdQuery_Ok()
        {
            //Arrenge
            Guid id = Guid.NewGuid();

            _userdataMock.Setup(p => p.GetUser(It.IsIn(id)))
                .Returns(new User
                {
                    Id = id,
                    Email = "test@test.com",
                    Username = "test"
                });

            User userExpectation = new User
            {
                Id = id,
                Email = "test@test.com",
                Username = "test"
            };

            //Act
            User user = new GetUserByIdQuery(_userdataMock.Object).Execute(new GetUserByIdQueryRequest
            {
                Id = id
            });

            //Assert
            _userdataMock.Verify(p => p.GetUser(It.IsIn(id)), Times.Once);

            Assert.IsTrue(userExpectation.Id == user.Id);
            Assert.IsTrue(userExpectation.Email == user.Email);
            Assert.IsTrue(userExpectation.Username == user.Username);
        }
    }
}
