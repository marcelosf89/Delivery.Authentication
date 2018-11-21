using Delivery.Authentication.Application.Query.UserManager;
using Delivery.Authentication.Application.Query.UserManager.Models;
using Delivery.Authentication.Crosscutting.Helper;
using Delivery.Authentication.Crosscutting.Response.UserManagement;
using Delivery.Authentication.Infrastructure.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using System.Text;

namespace Delivery.Authentication.Application.Query.Tests.UserManager
{
    [TestClass]
    public class GetUserAuthByUsernameAndPasswordQueryTest
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
        public void GetUserAuthByUsernameAndPasswordQuery_RerturnNull()
        {
            //Arrenge
            string userName = "testUser";
            string password = "Password";
            string passwordHash = "hwfuiewhfuiwerhfiu";

            _passwordHashMock.Setup(p => p.Converter(It.IsIn(password), It.IsAny<Encoding>()))
                    .Returns(passwordHash);

            _userdataMock.Setup(p => p.GetUserAuthInfo(It.IsIn(userName), It.IsIn(passwordHash)));

            //Act
            UserAuthInfoResponse user = new GetUserAuthByUsernameAndPasswordQuery(_userdataMock.Object, _passwordHashMock.Object)
                .Execute(new GetUserAuthByUsernameAndPasswordQueryRequest
                {
                    Username = userName,
                    Password = password
                });

            //Assert
            Assert.IsNull(user);
        }

        [TestMethod]
        public void GetUserAuthByUsernameAndPasswordQuery_RerturnUser()
        {
            //Arrenge
            string userName = "testUser";
            string password = "Password";
            string passwordHash = "hwfuiewhfuiwerhfiu";

            _passwordHashMock.Setup(p => p.Converter(It.IsIn(password), It.IsAny<Encoding>()))
                    .Returns(passwordHash);

            _userdataMock.Setup(p => p.GetUserAuthInfo(It.IsIn(userName), It.IsIn(passwordHash))).Returns(new UserAuthInfoResponse
            {
                Email = "email",
                Username = userName,
                Claims = new string[] { "a", "b" }
            });

            //Act
            UserAuthInfoResponse user = new GetUserAuthByUsernameAndPasswordQuery(_userdataMock.Object, _passwordHashMock.Object)
                .Execute(new GetUserAuthByUsernameAndPasswordQueryRequest
                {
                    Username = userName,
                    Password = password
                });

            //Assert
            Assert.IsNotNull(user);
            Assert.IsTrue(user.Claims.Count() == 2);
            Assert.IsTrue(user.Username.Equals(userName));
        }
    }
}
