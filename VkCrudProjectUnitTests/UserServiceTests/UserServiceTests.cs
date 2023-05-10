using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkCrudProject.Context;
using VkCrudProject.DTOs;
using VkCrudProject.Interfaces;
using VkCrudProject.Models;
using VkCrudProject.Profiles;
using VkCrudProject.Services;

namespace VkCrudProjectUnitTests.UserServiceTests
{
    public class UserServiceTests
    {

        private IUserService _userService;
        private IUserRepository _userRepository;

        public UserServiceTests()
        {
            UserContext? userContext = null;
            InMemoryDbBuilder dbBuilder = new InMemoryDbBuilder();
            IUserStateRepository userStateRepository = dbBuilder.GetInMemoryUserStateRepository(userContext);
            IUserGroupRepository userGroupRepository = dbBuilder.GetInMemoryUserGroupRepository(userContext);
            _userRepository = dbBuilder.GetInMemoryUserRepository(userContext, userGroupRepository, userStateRepository);
            var profile = new UserProfile();
            var configuration = new MapperConfiguration(cfg => cfg.AddProfile(profile));
            IMapper mapper = new Mapper(configuration);
            _userService = new UserService(_userRepository, userGroupRepository, userStateRepository, mapper);
        }

        [Fact]
        public async Task RegisterUserTest()
        {
            UserToCreate user1ToAdd = new UserToCreate { Login = "User1", Password = "1234"};
            UserToCreate user1ToAddClone = new UserToCreate { Login = "User1", Password = "1234"};
            UserToCreate user2ToAdd = new UserToCreate { Login = "User2", Password = "1234"};

            User user1ToAddExpected = new User { Id = 2, Login = "User1", Password = "1234", GroupId = 2, StateId = 1 };
            User user2ToAddExpected = new User { Id = 3, Login = "User2", Password = "1234", GroupId = 2, StateId = 1};

            await _userService.RegistrationAsync(user1ToAdd, 2);

            await Assert.ThrowsAsync<ArgumentException>(() => _userService.RegistrationAsync(user1ToAddClone, 2));
            await Assert.ThrowsAsync<ArgumentException>(() => _userService.RegistrationAsync(user2ToAdd, 1));
            
            await _userService.RegistrationAsync(user2ToAdd, 2);

            Assert.Equal(user1ToAddExpected.Id, _userRepository.GetUserByIdAsync(2).Result.Id);
            Assert.Equal(user1ToAddExpected.Login, _userRepository.GetUserByIdAsync(2).Result.Login);
            Assert.Equal(user1ToAddExpected.Password, _userRepository.GetUserByIdAsync(2).Result.Password);
            Assert.Equal(user1ToAddExpected.GroupId, _userRepository.GetUserByIdAsync(2).Result.GroupId);
            Assert.Equal((uint)1, _userRepository.GetUserByIdAsync(2).Result.StateId);


            Assert.Equal(user2ToAddExpected.Id, _userRepository.GetUserByIdAsync(3).Result.Id);
            Assert.Equal(user2ToAddExpected.Login, _userRepository.GetUserByIdAsync(3).Result.Login);
            Assert.Equal(user2ToAddExpected.Password, _userRepository.GetUserByIdAsync(3).Result.Password);
            Assert.Equal(user2ToAddExpected.GroupId, _userRepository.GetUserByIdAsync(3).Result.GroupId);
            Assert.Equal((uint)1, _userRepository.GetUserByIdAsync(2).Result.StateId);
        }

    }
}
