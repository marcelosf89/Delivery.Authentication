using Delivery.Authentication.Application.Command.UserManager;
using Delivery.Authentication.Application.Command.UserManager.Models;
using Delivery.Authentication.Crosscutting.Exceptions;
using Delivery.Authentication.Crosscutting.Helper;
using Delivery.Authentication.Crosscutting.Request.UserManagement;
using Delivery.Authentication.Domain.Model;
using Delivery.Authentication.Infrastructure.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Text;

namespace Delivery.Authentication.Application.Command.Tests.UserManager
{
    [TestClass]
    public class SaveUserCommnadTest
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
        [ExpectedException(typeof(ResponseException), "The username already exists")]
        public void SaveUserCommnad_ThrowUsernameExists()
        {
            //Arrenge
            _userdataMock.Setup(p => p.GetUserByUsername(It.IsIn("test")))
                                .Returns(new User
                                {
                                    Username = "test",
                                    Email = "test2@test.com"
                                });

            //Act
            new SaveUserCommand(_userdataMock.Object, _passwordHashMock.Object).Execute(new SaveUserRequest
            {
                Username = "test",
                Email = "test@test.com"
            });
        }

        [TestMethod]
        [ExpectedException(typeof(ResponseException), "The email already exists")]
        public void SaveUserCommnad_ThrowEmailExists()
        {
            //Arrenge
            _userdataMock.Setup(p => p.GetUserByEmail(It.IsIn("test@test.com")))
                            .Returns(new User
                            {
                                Username = "test2",
                                Email = "test@test.com"
                            });

            //Act
            new SaveUserCommand(_userdataMock.Object, _passwordHashMock.Object).Execute(new SaveUserRequest
            {
                Username = "test",
                Email = "test@test.com"
            });
        }

        [TestMethod]
        public void SaveUserCommnad_Ok()
        {
            //Arrenge
            User user = new User
            {
                Id = Guid.NewGuid(),
                Email = "test@test.com",
                Username = "test",
                FirstName = "FirstName",
                LastName = "LastName",
                Phone = "Phone",
                Password = "Password"
            };

            User userExcpectation = new User
            {
                Email = "test@test.com",
                Username = "test",
                FirstName = "FirstName",
                LastName = "LastName",
                Phone = "Phone",
                Password = "User.Password + x"
            };

            _userdataMock.Setup(p => p.GetUserByUsername(It.IsAny<string>()));
            _userdataMock.Setup(p => p.GetUserByEmail(It.IsAny<string>()));

            _passwordHashMock.Setup(p => p.Converter(It.IsIn("Password"), It.IsAny<Encoding>()))
                .Returns(userExcpectation.Password);

            //Act
            new SaveUserCommand(_userdataMock.Object, _passwordHashMock.Object).Execute(new SaveUserRequest
            {
                Email = userExcpectation.Email,
                FirstName = userExcpectation.FirstName,
                LastName = userExcpectation.LastName,
                Password = "Password",
                Phone = userExcpectation.Phone,
                Username = userExcpectation.Username
            });

            //Assert
            _passwordHashMock.Verify(p => p.Converter(It.IsAny<string>(), It.IsAny<Encoding>()), Times.Once);

            _userdataMock.Verify(p => p.Save(It.Is((User u) =>
                                                u.Password == userExcpectation.Password &&
                                                u.Email == userExcpectation.Email &&
                                                u.Username == userExcpectation.Username &&
                                                u.FirstName == userExcpectation.FirstName &&
                                                u.LastName == userExcpectation.LastName &&
                                                u.Phone == userExcpectation.Phone &&
                                                u.DeletionDate == null
                                                )));
        }
    }
}
