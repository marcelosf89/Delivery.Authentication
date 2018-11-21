using Delivery.Authentication.Application.Query.UserManager;
using Delivery.Authentication.Application.Query.UserManager.Models;
using Delivery.Authentication.Infrastructure.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Delivery.Authentication.Application.Query.Tests.UserManager
{
    [TestClass]
    public class GetUserClaimByUserIdQueryTest
    {
        private Mock<IUserData> _userdataMock;

        [TestInitialize]
        public void Init()
        {
            _userdataMock = new Mock<IUserData>();
        }

        [TestMethod]
        public void GetUserClaimByUserIdQuery_RerturnNull()
        {
            //Arrenge
            Guid id = Guid.NewGuid();

            _userdataMock.Setup(p => p.GetClaimsByUserId(It.IsAny<Guid>()));

            //Act
           IEnumerable<string> claims = new GetUserClaimByUserIdQuery(_userdataMock.Object)
                .Execute(new GetUserClaimByUserIdQueryRequest
           {
               UserId = id
           });

            //Assert
            Assert.IsNull(claims);
        }

        [TestMethod]
        public void GetUserClaimByUserIdQuery_RerturnClaims()
        {
            //Arrenge
            Guid id = Guid.NewGuid();

            _userdataMock.Setup(p => p.GetClaimsByUserId(It.IsIn(id))).Returns( new string[] { "a", "b", "c"});

            //Act
            IEnumerable<string> claims = new GetUserClaimByUserIdQuery(_userdataMock.Object).Execute(
                new GetUserClaimByUserIdQueryRequest
                {
                    UserId = id
                });

            //Assert
            Assert.IsNotNull(claims);
            Assert.IsTrue(claims.Count() ==  3);
        }
    }
}
