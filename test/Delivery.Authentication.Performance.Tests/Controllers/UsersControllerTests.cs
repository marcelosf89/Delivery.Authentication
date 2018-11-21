using BenchmarkDotNet.Attributes;
using Delivery.Authentication.Crosscutting.Helper;
using Delivery.Authentication.Infrastructure.Data;
using Delivery.Authentication.Api.Controllers;
using Moq;
using Delivery.Authentication.Application.Query.UserManager;
using Delivery.Authentication.Application.Command.UserManager;
using Delivery.Authentication.Crosscutting.Request.UserManagement;
using Delivery.Authentication.Domain.Model;
using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Mvc;
using Delivery.Authentication.Crosscutting.Exceptions;

namespace Delivery.Authentication.Performance.Tests.Controllers
{
    [Config(typeof(ManualConfiguration))]
    public class UsersControllerTests
    {
        private Mock<IUserData> _userDataMock;
        private IUserData _userData;
        private IPasswordHash _passwordHash;
        private UsersController _usersController;

        public Guid _userId = Guid.NewGuid();

        private User _userBase;

        [GlobalSetup()]
        public void GlobalSetupGetUser()
        {
            _userDataMock = new Mock<IUserData>();

            _userBase = UsersData(_userId);

            _userData = _userDataMock.Object;

            _passwordHash = new PasswordHash();
            _usersController = new UsersController(
                new GetUserByIdQuery(_userData),
                new SaveUserCommand(_userData, _passwordHash),
                new UpdateUserCommand(_userData, _passwordHash),
                new DeleteUserCommand(_userData),
                new AddClaimUserCommnad(_userData),
                new RemoveClaimUserCommnad(_userData));
        }

        [Benchmark]
        [BenchmarkCategory("UserController", "Save", "Ok")]
        public void CreateUser_Ok()
        {
            _usersController.Save(new SaveUserRequest
            {
                Email = _userBase.Email + "new.User",
                FirstName = "Test",
                LastName = "Performance",
                Password = "Test@Performance!",
                Phone = "351999999999",
                Username = _userBase.Username + "new.User"
            });
        }

        [Benchmark]
        [BenchmarkCategory("UserController", "Save", "UserEmailExists")]
        public void CreateUserUserEmailExists()
        {
            try
            {
                _usersController.Save(new SaveUserRequest
                {
                    Email = _userBase.Email,
                    FirstName = "Test",
                    LastName = "Performance",
                    Password = "Test@Performance!",
                    Phone = "351999999999",
                    Username = _userBase.Username + "new.User"
                });
            }
            catch (ResponseException) { }
        }

        [Benchmark]
        [BenchmarkCategory("UserController", "Save", "UsernameExists")]
        public void CreateUserUsernameExists()
        {
            try
            {
                _usersController.Save(new SaveUserRequest
                {
                    Email = _userBase.Email + "new.user",
                    FirstName = "Test",
                    LastName = "Performance",
                    Password = "Test@Performance!",
                    Phone = "351999999999",
                    Username = _userBase.Username
                });
            }
            catch (ResponseException) { }
        }



        [Benchmark]
        [BenchmarkCategory("UserController", "UpdateUser", "UserNotExists")]
        public void UpdateUserUserNotExists()
        {
            try
            {
                _usersController.Update(new UpdateUserRequest
                {
                    Id = Guid.NewGuid(),
                    Email = "test@test.co,",
                    FirstName = "Test",
                    LastName = "Performance",
                    Phone = "351999999999",
                    Password = "Password",
                    Username = "test.performance01"
                });
            }
            catch (ResponseException) { }
        }

        [Benchmark]
        [BenchmarkCategory("UserController", "Get", "UserNotExists")]
        public void GetUserUserNotExists()
        {
            try
            {
                _usersController.Get(Guid.NewGuid());
            }
            catch (ResponseException) { }
        }

        [Benchmark]
        [BenchmarkCategory("UserController", "AddClaim", "UserNotExists")]
        public void AddClaimUserNotExists()
        {
            try
            {
                _usersController.AddClaim(new AddClaimUserRequest
                {
                    Username = "new",
                    Claims = new string[] { "newClaim" }
                });
            }
            catch (ResponseException) { }
        }


        [Benchmark]
        [BenchmarkCategory("UserController", "Delete", "UserNotExists")]
        public void DeleteUserNotExists()
        {
            try
            {
                _usersController.Delete(Guid.NewGuid());
            }
            catch (ResponseException) { }
        }


        [Benchmark]
        [BenchmarkCategory("UserController", "AddClaim", "Ok")]
        public void AddClaim_Ok()
        {
            _usersController.AddClaim(new AddClaimUserRequest
            {
                Username = "test.performance01",
                Claims = new string[] { "newClaim" }
            });
        }


        [Benchmark]
        [BenchmarkCategory("UserController", "Delete", "Ok")]
        public void Delete_Ok()
        {
            _usersController.Delete(_userId);
        }

        [Benchmark]
        [BenchmarkCategory("UserController", "UpdateUserWithoutChangePassword", "Ok")]
        public void UpdateUserWithoutChangePassword_Ok()
        {
            _usersController.Update(new UpdateUserRequest
            {
                Id = _userId,
                Email = "test@test.co,",
                FirstName = "Test",
                LastName = "Performance",
                Phone = "351999999999",
                Username = "test.performance01"
            });
        }

        [Benchmark]
        [BenchmarkCategory("UserController", "UpdateUserWithChangePassword", "Ok")]
        public void UpdateUserWithChangePassword_Ok()
        {
            _usersController.Update(new UpdateUserRequest
            {
                Id = _userId,
                Email = "test@test.co,",
                FirstName = "Test",
                LastName = "Performance",
                Phone = "351999999999",
                Password = "Password",
                Username = "test.performance01"
            });
        }

        [Benchmark]
        [BenchmarkCategory("UserController", "Get", "Ok")]
        public IActionResult GetUser_Ok()
        {
            return _usersController.Get(_userId);
        }


        public IEnumerable<Guid> UserIds()
        {
            yield return Guid.NewGuid();
        }

        public User UsersData(Guid userId)
        {
            User user = new User
            {
                Id = userId,
                Creation = DateTime.Now,
                DeletionDate = null,
                LastAccess = DateTime.Now,
                Email = "test@test.co,",
                FirstName = "Test",
                LastName = "Performance",
                Password = "Test@Performance!",
                Phone = "351999999999",
                Username = "test.performance01"
            };

            _userDataMock.Setup(p => p.GetUser(It.IsIn(user.Id)))
                .Returns(user);

            _userDataMock.Setup(p => p.GetUserByUsername( It.IsIn(user.Username)));
            _userDataMock.Setup(p => p.GetUserByEmail(It.IsIn(user.Email)));

            return user;
        }

    }
}
