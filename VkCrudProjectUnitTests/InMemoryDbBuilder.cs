using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VkCrudProject.Context;
using VkCrudProject.Interfaces;
using VkCrudProject.Repositories;

namespace VkCrudProjectUnitTests
{
    public class InMemoryDbBuilder
    {

        private Dictionary<string, string> myConfiguration = new Dictionary<string, string>()
            {
                { "BaseUser:Id", "1"},
                { "BaseUser:Login", "Admin" },
                { "BaseUser:Password", "1234" }
            };
        private IConfigurationRoot configuration;

        public InMemoryDbBuilder()
        {
            configuration = new ConfigurationBuilder().AddInMemoryCollection(myConfiguration).Build();
        }
        private UserContext GetInMemoryDb(UserContext userContext, IConfiguration configuration)
        {
            DbContextOptions<UserContext> options;
            var builder = new DbContextOptionsBuilder<UserContext>();
            var databaseName = "userContext_" + DateTime.Now.ToFileTimeUtc();
            builder = builder.UseInMemoryDatabase(databaseName: databaseName);
            options = builder.Options;
            userContext = new UserContext(options, configuration);
            userContext.Database.EnsureDeleted();
            userContext.Database.EnsureCreated();
            return userContext;
        }

        public IUserRepository GetInMemoryUserRepository(UserContext userContext, IUserGroupRepository userGroupRepository, IUserStateRepository userStateRepository)
        {
            return new UserRepository(GetInMemoryDb(userContext, configuration), userStateRepository, userGroupRepository);
        }
        public IUserStateRepository GetInMemoryUserStateRepository(UserContext userContext)
        {
            return new UserStateRepository(GetInMemoryDb(userContext, configuration));
        }
        public IUserGroupRepository GetInMemoryUserGroupRepository(UserContext userContext)
        {
            return new UserGroupRepository(GetInMemoryDb(userContext, configuration));
        }
    }
}
