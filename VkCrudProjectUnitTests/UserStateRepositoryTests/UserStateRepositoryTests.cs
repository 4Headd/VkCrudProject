using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkCrudProject.Context;
using VkCrudProject.Interfaces;
using VkCrudProject.Models;

namespace VkCrudProjectUnitTests.UserStateRepositoryTests
{
    public class UserStateRepositoryTests
    {
        private IUserStateRepository sut;

        public UserStateRepositoryTests()
        {
            UserContext? userContext = null;
            InMemoryDbBuilder dbBuilder = new InMemoryDbBuilder();
            sut = dbBuilder.GetInMemoryUserStateRepository(userContext);
        }

        [Fact]
        public async Task GetStateTest()
        {
            UserState userState1Expected = new UserState { Id = 1, Status = UserStatus.Active, Description = "Active user" };
            UserState userState2Expected = new UserState { Id = 2, Status = UserStatus.Blocked, Description = "Blocked user" };


            using (sut)
            {
                Assert.Equal(userState1Expected.Id, sut.GetUserStateByIdAsync(1).Result.Id);
                Assert.Equal(userState1Expected.Status, sut.GetUserStateByIdAsync(1).Result.Status);
                Assert.Equal(userState1Expected.Description, sut.GetUserStateByIdAsync(1).Result.Description);


                Assert.Equal(userState2Expected.Id, sut.GetUserStateByIdAsync(2).Result.Id);
                Assert.Equal(userState2Expected.Status, sut.GetUserStateByIdAsync(2).Result.Status);
                Assert.Equal(userState2Expected.Description, sut.GetUserStateByIdAsync(2).Result.Description);


            }

        }

        [Fact]
        public async Task AddStateTest()
        {
            UserState userState1ToAdd = new UserState { Id = 3, Status = UserStatus.Blocked, Description = "Another blocked status" };
            UserState userState1ToAddClone = new UserState { Id = 3, Status = UserStatus.Blocked, Description = "Another blocked status" };



            using (sut)
            {
                await sut.AddUserStateAsync(userState1ToAdd);
                await Assert.ThrowsAsync<InvalidOperationException>(() => sut.AddUserStateAsync(userState1ToAddClone));

                Assert.Equal(userState1ToAdd.Id, sut.GetUserStateByIdAsync(3).Result.Id);
                Assert.Equal(userState1ToAdd.Status, sut.GetUserStateByIdAsync(3).Result.Status);
                Assert.Equal(userState1ToAdd.Description, sut.GetUserStateByIdAsync(3).Result.Description);

            }

        }
    }
}
