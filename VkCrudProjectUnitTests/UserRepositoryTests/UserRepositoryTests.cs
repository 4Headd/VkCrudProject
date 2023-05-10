using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using VkCrudProject.Context;
using VkCrudProject.Interfaces;
using VkCrudProject.Models;
using VkCrudProject.Repositories;

namespace VkCrudProjectUnitTests.UserRepositoryTests
{
    public class UserRepositoryTests
    {
        private IUserRepository sut;

        
        public UserRepositoryTests()
        {
            UserContext? userContext = null;
            InMemoryDbBuilder dbBuilder = new InMemoryDbBuilder();
            IUserStateRepository userStateRepository = dbBuilder.GetInMemoryUserStateRepository(userContext);
            IUserGroupRepository userGroupRepository = dbBuilder.GetInMemoryUserGroupRepository(userContext);
            sut = dbBuilder.GetInMemoryUserRepository(userContext, userGroupRepository, userStateRepository);
        }


        [Fact]
        public void GetUserTest()
        {
            User user1Expected = new User { Id = 1, Login = "Admin", Password = "1234", GroupId = 1, StateId = 1 };

            using (sut)
            {
                Assert.Equal(user1Expected.Id, sut.GetUserByIdAsync(1).Result.Id);
                Assert.Equal(user1Expected.Login, sut.GetUserByIdAsync(1).Result.Login);
                Assert.Equal(user1Expected.Password, sut.GetUserByIdAsync(1).Result.Password);
                Assert.Equal(user1Expected.GroupId, sut.GetUserByIdAsync(1).Result.GroupId);
                Assert.Equal(user1Expected.StateId, sut.GetUserByIdAsync(1).Result.StateId);



                Assert.Equal(user1Expected.Id, sut.GetUserByLoginAsync("Admin").Result.Id);
                Assert.Equal(user1Expected.Login, sut.GetUserByLoginAsync("Admin").Result.Login);
                Assert.Equal(user1Expected.Password, sut.GetUserByLoginAsync("Admin").Result.Password);
                Assert.Equal(user1Expected.GroupId, sut.GetUserByLoginAsync("Admin").Result.GroupId);
                Assert.Equal(user1Expected.StateId, sut.GetUserByLoginAsync("Admin").Result.StateId);

                

                Assert.Single(sut.GetAllUsersAsync().Result.ToList());
            }
        }



        [Fact] 
        public async Task GetUserPageTest()
        {
            List<User> users = new List<User>()
            {
                new User { Id = 1, Login = "Admin", Password = "1234", GroupId = 1, StateId = 1 },
                new User() { Id = 2, Login = "User2", Password = "12344", GroupId = 2, StateId = 1 },
                new User() { Id = 3, Login = "User3", Password = "123445", GroupId = 2, StateId = 1 },
                new User() { Id = 4, Login = "User4", Password = "123432", GroupId = 2, StateId = 1 },
                new User() { Id = 5, Login = "User5", Password = "1234125", GroupId = 2, StateId = 1 },
                new User() { Id = 6, Login = "User6", Password = "1234756", GroupId = 2, StateId = 1 },
                new User() { Id = 7, Login = "User7", Password = "1234967", GroupId = 2, StateId = 1 },
                new User() { Id = 8, Login = "User8", Password = "123049", GroupId = 2, StateId = 1 },
                new User() { Id = 9, Login = "User9", Password = "1234978", GroupId = 2, StateId = 1 },
                new User() { Id = 10, Login = "User10", Password = "123488", GroupId = 2, StateId = 2 },
            };

            using (sut)
            {
                for (int i = 1; i < users.Count; i++)
                {
                    await sut.RegisterUserAsync(users[i]);
                }

                var listOfUsers1 = sut.GetUsersAsync(new UserParameters { PageNumber = 1, PageSize = 20}).Result.ToList();

                Assert.Equal(10, listOfUsers1.Count);

                Assert.Equal(users[0].Id, listOfUsers1[0].Id);
                Assert.Equal(users[0].Login, listOfUsers1[0].Login);
                Assert.Equal(users[0].Password, listOfUsers1[0].Password);
                Assert.Equal(users[0].GroupId, listOfUsers1[0].GroupId);
                Assert.Equal(users[0].StateId, listOfUsers1[0].StateId);

                for (int i = 1; i < listOfUsers1.Count; i++)
                {
                    Assert.Equal(listOfUsers1[i], await sut.GetUserByIdAsync((uint)i + 1));
                }

                var listOfUsers2 = sut.GetUsersAsync(new UserParameters { PageNumber = 1, PageSize = 1 }).Result.ToList();

                Assert.Single(listOfUsers2);

                Assert.Equal(users[0].Id, listOfUsers2[0].Id);
                Assert.Equal(users[0].Login, listOfUsers2[0].Login);
                Assert.Equal(users[0].Password, listOfUsers2[0].Password);
                Assert.Equal(users[0].GroupId, listOfUsers2[0].GroupId);
                Assert.Equal(users[0].StateId, listOfUsers2[0].StateId);

                var listOfUsers3 = sut.GetUsersAsync(new UserParameters { PageNumber = 4, PageSize = 3 }).Result.ToList();

                Assert.Single(listOfUsers3);

                Assert.Equal(users[9], listOfUsers3[0]);

                var listOfUsers4 = sut.GetUsersAsync(new UserParameters { PageNumber = 2, PageSize = 6 }).Result.ToList();

                Assert.Equal(4, listOfUsers4.Count);


                for (int i = 1; i < listOfUsers4.Count; i++)
                {
                    Assert.Equal(listOfUsers4[i], await sut.GetUserByIdAsync((uint)i + 7));
                }


                var listOfUsers5 = sut.GetUsersAsync(new UserParameters { PageNumber = 3, PageSize = 10 }).Result.ToList();

                Assert.Empty(listOfUsers5);

                var listOfUsers6 = sut.GetUsersAsync(new UserParameters { PageNumber = 2, PageSize = 3 }).Result.ToList();

                Assert.Equal(3, listOfUsers6.Count);

                for (int i = 3; i < listOfUsers6.Count; i++)
                {
                    Assert.Equal(listOfUsers4[i], await sut.GetUserByIdAsync((uint)i + 4));
                }
            }
        }

        [Fact]
        public async Task RegisterUserTest()
        {
            User user1ToAdd = new User { Id = 2, Login = "User1", Password = "1234", GroupId = 2, StateId = 1 };
            User user2ToAdd = new User { Id = 3, Login = "User2", Password = "1234", GroupId = 2, StateId = 1 };
            User user3ToAdd = new User { Id = 4, Login = "User3", Password = "1234", GroupId = 2, StateId = 1 };

            using (sut)
            {
                await sut.RegisterUserAsync(user1ToAdd);
                await sut.RegisterUserAsync(user2ToAdd);
                await sut.RegisterUserAsync(user3ToAdd);


                Assert.Equal(user1ToAdd.Id, sut.GetUserByIdAsync(2).Result.Id);
                Assert.Equal(user1ToAdd.Login, sut.GetUserByIdAsync(2).Result.Login);
                Assert.Equal(user1ToAdd.Password, sut.GetUserByIdAsync(2).Result.Password);
                Assert.Equal(user1ToAdd.GroupId, sut.GetUserByIdAsync(2).Result.GroupId);
                Assert.Equal((uint) 1, sut.GetUserByIdAsync(2).Result.StateId);


                var listOfUsers = sut.GetAllUsersAsync().Result.ToList();

                Assert.Equal(user2ToAdd.Id, sut.GetUserByIdAsync(3).Result.Id);
                Assert.Equal(user2ToAdd.Login, sut.GetUserByIdAsync(3).Result.Login);
                Assert.Equal(user2ToAdd.Password, sut.GetUserByIdAsync(3).Result.Password);
                Assert.Equal(user2ToAdd.GroupId, sut.GetUserByIdAsync(3).Result.GroupId);
                Assert.Equal((uint)1, sut.GetUserByIdAsync(2).Result.StateId);

                Assert.Equal(user3ToAdd.Id, sut.GetUserByIdAsync(4).Result.Id);
                Assert.Equal(user3ToAdd.Login, sut.GetUserByIdAsync(4).Result.Login);
                Assert.Equal(user3ToAdd.Password, sut.GetUserByIdAsync(4).Result.Password);
                Assert.Equal(user3ToAdd.GroupId, sut.GetUserByIdAsync(4).Result.GroupId);
                Assert.Equal((uint)1, sut.GetUserByIdAsync(2).Result.StateId);
            }
        }


        [Fact]
        public async Task BlockUserTest()
        {
            User user1ToAdd = new User { Id = 2, Login = "User1", Password = "1234", GroupId = 2, StateId = 1 };


            using (sut)
            {
                await sut.RegisterUserAsync(user1ToAdd);


                Assert.True(sut.BlockUserAsync(2).Result);
                Assert.Equal((uint) 2, sut.GetUserByIdAsync(2).Result.StateId);
            }

        }


        [Fact]
        public async Task ActivateUserTest()
        {
            User user1ToAdd = new User { Id = 2, Login = "User1", Password = "1234", GroupId = 2, StateId = 2 };


            using (sut)
            {
                await sut.RegisterUserAsync(user1ToAdd);

                await sut.BlockUserAsync(2);

                Assert.True(sut.ActivateUserAsync(2).Result);
                Assert.Equal((uint)1, sut.GetUserByIdAsync(2).Result.StateId);
            }
        }
    }
}