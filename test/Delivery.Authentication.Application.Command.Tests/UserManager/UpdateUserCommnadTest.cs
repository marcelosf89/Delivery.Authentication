using System;
using System.Text;
using Delivery.Authentication.Application.Command.UserManager;
using Delivery.Authentication.Crosscutting.Exceptions;
using Delivery.Authentication.Crosscutting.Helper;
using Delivery.Authentication.Crosscutting.Request.UserManagement;
using Delivery.Authentication.Domain.Model;
using Delivery.Authentication.Infrastructure.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Delivery.Authentication.Application.Command.Tests.UserManager
{
    [TestClass]
    public class UpdateUserCommnadTest
    {
        private Mock<IUserData> _userdataMock;
        private Mock<IPasswordHash> _passwordHashMock;

        [TestInitialize]
        public void Init()
        {
            _userdataMock = new Mock<IUserData>();
            _passwordHashMock = new Mock<IPasswordHash>();
        }

        [TestMethod]
        [ExpectedException(typeof(ResponseException), "The user id does not exist")]
        public void UpdateUserCommand_InvalidId_ThrowResponseException400()
        {
            //Arrenge
            Guid id = Guid.NewGuid();

            _userdataMock.Setup(p => p.GetUser(It.IsAny<Guid>()));

            //Act
            new UpdateUserCommand(_userdataMock.Object, _passwordHashMock.Object).Execute(new UpdateUserRequest
            {
                Id = id
            });
        }

        [TestMethod]
        public void UpdateUserCommand_ValidId_NotUpdatePassword_NotException()
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

            User userExcpectation = new User
            {
                Id = user.Id,
                Email = "user.Email + x",
                Username = "user.Username + x",
                FirstName = "user.FirstName + x",
                LastName = "user.LastName + x",
                Phone = "user.Phone + x"
            };

            _userdataMock.Setup(p => p.GetUser(It.IsIn(user.Id)))
                .Returns(user);

            //Act
            new UpdateUserCommand(_userdataMock.Object, _passwordHashMock.Object).Execute(new UpdateUserRequest
            {
                Id = userExcpectation.Id,
                Email = userExcpectation.Email,
                FirstName = userExcpectation.FirstName,
                LastName = userExcpectation.LastName,
                Password = String.Empty,
                Phone = userExcpectation.Phone,
                Username = userExcpectation.Username
            });

            //Assert
            _passwordHashMock.Verify(p => p.Converter(It.IsAny<string>(), It.IsAny<Encoding>()), Times.Never);

            _userdataMock.Verify(p => p.Update(It.Is((User u) =>
                u.Id == userExcpectation.Id &&
                u.Email == userExcpectation.Email &&
                u.FirstName == userExcpectation.FirstName &&
                u.LastName == userExcpectation.LastName &&
                u.Password == userExcpectation.Password &&
                u.Phone == userExcpectation.Phone &&
                u.Username == userExcpectation.Username
            )));
        }

        [TestMethod]
        public void UpdateUserCommand_Ok()
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

            User userExcpectation = new User
            {
                Id = user.Id,
                Email = "user.Email + x",
                Username = "user.Username + x",
                FirstName = "user.FirstName + x",
                LastName = "user.LastName + x",
                Phone = "user.Phone + x",
                Password = "User.Password + x"
            };

            _userdataMock.Setup(p => p.GetUser(It.IsIn(user.Id)))
                .Returns(user);

            _passwordHashMock.Setup(p => p.Converter(It.IsIn("Password"), It.IsAny<Encoding>()))
                .Returns(userExcpectation.Password);

            //Act
            new UpdateUserCommand(_userdataMock.Object, _passwordHashMock.Object).Execute(new UpdateUserRequest
            {
                Id = userExcpectation.Id,
                Email = userExcpectation.Email,
                FirstName = userExcpectation.FirstName,
                LastName = userExcpectation.LastName,
                Password = "Password",
                Phone = userExcpectation.Phone,
                Username = userExcpectation.Username
            });

            //Assert
            _passwordHashMock.Verify(p => p.Converter(It.IsAny<string>(), It.IsAny<Encoding>()), Times.Once);

            _userdataMock.Verify(p => p.Update(It.Is((User u) =>
                u.Id == userExcpectation.Id &&
                u.Email == userExcpectation.Email &&
                u.FirstName == userExcpectation.FirstName &&
                u.LastName == userExcpectation.LastName &&
                u.Password == userExcpectation.Password &&
                u.Phone == userExcpectation.Phone &&
                u.Username == userExcpectation.Username
            )));
        }
    }
}
