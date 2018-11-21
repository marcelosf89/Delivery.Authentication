using Delivery.Authentication.Application.Query.IdentityManager;
using Delivery.Authentication.Application.Query.IdentityManager.Model;
using Delivery.Authentication.Domain.Model;
using Delivery.Authentication.Infrastructure.Data.IdentityManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Delivery.Authentication.Application.Query.Tests.IdentityManager
{
    [TestClass]
    public class GetIdentityApiByApiScopesQueryTest
    {
        private Mock<IIdentityApiData> _identityApiDataMock;

        [TestInitialize]
        public void Init()
        {
            _identityApiDataMock = new Mock<IIdentityApiData>();
        }

        [TestMethod]
        public async Task GetIdentityApiByApiScopesQuery_EmptyScope_ReturnNull()
        {
            //Arrenge

            //Act
            IEnumerable<IdentityApi> response = await new GetIdentityApiByApiScopesQuery(_identityApiDataMock.Object).ExecuteAsync(
                new GetIdentityApiByApiScopesQueryRequest
                {
                    ApiScope = null
                });

            //Assert
            Assert.IsNull(response);
        }

        [TestMethod]
        public async Task GetIdentityApiByApiScopesQuery_WithScopeInvalid_ReturnNull()
        {
            //Arrenge

            //Act
            IEnumerable<IdentityApi> response = await new GetIdentityApiByApiScopesQuery(_identityApiDataMock.Object).ExecuteAsync(
                new GetIdentityApiByApiScopesQueryRequest
                {
                    ApiScope = new string[]{ "scopeInvalid" }
                });

            //Asset
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Count() == 0);
        }

        [TestMethod]
        public async Task GetIdentityApiByApiScopesQuery_WithOneScopeValid_ReturnOneIdentityApi()
        {
            //Arrenge
            string scope1 = "scope.1";

            HashSet<IdentityApi> identities = new HashSet<IdentityApi>();

            identities.Add(new IdentityApi() { Code = scope1, Description = "scope1.description" });

            _identityApiDataMock.Setup(p => p.GetIdentityApiByApiScopesAsync(It.Is((string[] s) => s.Any(_ => _.Equals(scope1)))))
                .ReturnsAsync(identities);

            //Act
            IEnumerable<IdentityApi> response = await new GetIdentityApiByApiScopesQuery(_identityApiDataMock.Object).ExecuteAsync(
                new GetIdentityApiByApiScopesQueryRequest
                {
                    ApiScope = new string[] { scope1 }
                });

            //Assert
            Assert.IsNotNull(response);
            Assert.IsTrue(response.Count() == 1);
            Assert.IsTrue(response.FirstOrDefault().Description.Equals(identities.FirstOrDefault().Description));
        }

    }
}
