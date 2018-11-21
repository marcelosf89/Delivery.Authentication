using BenchmarkDotNet.Attributes;
using Delivery.Authentication.Infrastructure.Data;
using Delivery.Authentication.Api.Controllers;
using Moq;
using Delivery.Authentication.Application.Query.ClaimManager;
using Delivery.Authentication.Application.Command.ClaimManager;
using Delivery.Authentication.Domain.Model;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Delivery.Authentication.Crosscutting.Exceptions;
using Delivery.Authentication.Crosscutting.Request.ClaimManagement;

namespace Delivery.Authentication.Performance.Tests.Controllers
{
    [Config(typeof(ManualConfiguration))]
    public class ClaimsControllerTests
    {
        private Mock<IClaimData> _claimDataMock;
        private IClaimData _claimData;
        private ClaimsController _claimsController;

        public string _claimCode = "test.performance.01";

        private Claim _claimBase;

        [GlobalSetup()]
        public void GlobalSetupGetUser()
        {
            _claimDataMock = new Mock<IClaimData>();

            _claimBase = ClaimsData(_claimCode);

            _claimData = _claimDataMock.Object;

            _claimsController = new ClaimsController(
                new GetClaimByCodeQuery(_claimData),
                new SaveClaimCommand(_claimData),
                new UpdateClaimCommand(_claimData),
                new SetObsoleteClaimCommnad(_claimData));
        }

        [Benchmark]
        [BenchmarkCategory("ClaimController", "Save", "Ok")]
        public void CreateClaim_Ok()
        {
            _claimsController.Save(new SaveClaimRequest
            {
                Code = _claimBase.Code + "new.claim",
                Description = "Test Performance"
            });
        }

        [Benchmark]
        [BenchmarkCategory("ClaimController", "Save", "ClaimExists")]
        public void CreateClaim_ClaimExists()
        {
            try
            {
                _claimsController.Save(new SaveClaimRequest
                {
                    Code = _claimBase.Code,
                    Description = "Test Performance"
                });
            }
            catch (ResponseException) { }
        }

        [Benchmark]
        [BenchmarkCategory("ClaimController", "Get", "ClaimNotExists")]
        public void GetClaim_ClaimNotExists()
        {
            try
            {
                _claimsController.Get("test.performance.a");
            }
            catch (ResponseException) { }
        }

        [Benchmark]
        [BenchmarkCategory("ClaimController", "Get", "Ok")]
        public IActionResult GetClaim_Ok()
        {
            return _claimsController.Get(_claimCode);
        }

        [Benchmark]
        [BenchmarkCategory("ClaimController", "SetObsolete", "Ok")]
        public void SetObsoleteOk()
        {
            _claimsController.SetObsolete("test.performance");
        }

        [Benchmark]
        [BenchmarkCategory("ClaimController", "SetObsolete", "NotExist")]
        public void SetObsoleteNotExists()
        {
            _claimsController.SetObsolete("test.performance.a");
        }

        [Benchmark]
        [BenchmarkCategory("ClaimController", "UpdateUser", "ClaimNotExists")]
        public void UpdateClaim_ClaimNotExists()
        {
            try
            {
                _claimsController.Update(new UpdateClaimRequest
                {
                    Code = "test.performance.a",
                    Description = "test"
                });
            }
            catch (ResponseException) { }
        }

        [Benchmark]
        [BenchmarkCategory("ClaimController", "Update", "Ok")]
        public void UpdateClaim_Ok()
        {
            _claimsController.Update(new UpdateClaimRequest
            {
                Code = _claimCode,
                Description = "test@test.co,"
            });
        }

        public Claim ClaimsData(string code)
        {
            Claim claim = new Claim
            {
                Code = code,
                Description = "performance test in action",
                IsObsolete = false
            };

            _claimDataMock.Setup(p => p.GetClaim(It.IsIn(claim.Code)))
                .Returns(claim);

            _claimDataMock.Setup(p => p.HasClaim(It.IsIn(claim.Code)))
                .Returns(true);

            return claim;
        }

    }
}
