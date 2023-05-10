using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkCrudProject.Context;
using VkCrudProject.Interfaces;
using VkCrudProject.Models;
using VkCrudProject.Repositories;
using Microsoft.Extensions.Configuration;

namespace VkCrudProjectUnitTests.UserGroupRepositoryTests
{
    public class UserGroupRepositoryTests
    {
        private IUserGroupRepository sut;

        public UserGroupRepositoryTests()
        {
            UserContext? userContext = null;
            InMemoryDbBuilder dbBuilder = new InMemoryDbBuilder();
            sut = dbBuilder.GetInMemoryUserGroupRepository(userContext);
        }

        [Fact]
        public async Task GetGroupTest()
        {
            UserGroup userGroup1Expected = new UserGroup { Id = 1, Role = UserRole.Admin, Description = "Administrator: moderating and maintaining service" };
            UserGroup userGroup2Expected = new UserGroup { Id = 2, Role = UserRole.User, Description = "User" };


            using (sut)
            {
                Assert.Equal(userGroup1Expected.Id, sut.GetUserGroupByIdAsync(1).Result.Id);
                Assert.Equal(userGroup1Expected.Role, sut.GetUserGroupByIdAsync(1).Result.Role);
                Assert.Equal(userGroup1Expected.Description, sut.GetUserGroupByIdAsync(1).Result.Description);


                Assert.Equal(userGroup2Expected.Id, sut.GetUserGroupByIdAsync(2).Result.Id);
                Assert.Equal(userGroup2Expected.Role, sut.GetUserGroupByIdAsync(2).Result.Role);
                Assert.Equal(userGroup2Expected.Description, sut.GetUserGroupByIdAsync(2).Result.Description);


            }

        }

        [Fact]
        public async Task AddGroupTest()
        {
            UserGroup userGroup1ToAdd = new UserGroup { Id = 3, Role = UserRole.User, Description = "Another user" };
            UserGroup userGroup1ToAddClone = new UserGroup { Id = 3, Role = UserRole.User, Description = "Another user" };


            using (sut)
            {
                await sut.AddUserGroupAsync(userGroup1ToAdd);
                await Assert.ThrowsAsync<InvalidOperationException>(() => sut.AddUserGroupAsync(userGroup1ToAddClone));

                Assert.Equal(userGroup1ToAdd.Id, sut.GetUserGroupByIdAsync(3).Result.Id);
                Assert.Equal(userGroup1ToAdd.Role, sut.GetUserGroupByIdAsync(3).Result.Role);
                Assert.Equal(userGroup1ToAdd.Description, sut.GetUserGroupByIdAsync(3).Result.Description);

            }

        }

    }
}
