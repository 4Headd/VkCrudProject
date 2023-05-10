using Microsoft.EntityFrameworkCore;
using VkCrudProject.Context;
using VkCrudProject.Interfaces;
using VkCrudProject.Models;

namespace VkCrudProject.Repositories
{
    public class UserGroupRepository : IUserGroupRepository
    {
        private readonly UserContext _userContext;

        public UserGroupRepository(UserContext userContext)
        {
            _userContext = userContext;
        }

        public async Task AddUserGroupAsync(UserGroup userGroup)
        {
            await _userContext.UserGroups.AddAsync(userGroup);
            await SaveChanges();
        }

        public async Task<UserGroup> GetUserGroupByIdAsync(uint id)
        {
            return await _userContext.UserGroups.FirstOrDefaultAsync(userGroup_ => userGroup_.Id == id);
        }

        public async Task<bool> SaveChanges()
        {
            return (await _userContext.SaveChangesAsync() >= 0);
        }

        public void Dispose()
        {
            _userContext.Dispose();
        }
    }
}
