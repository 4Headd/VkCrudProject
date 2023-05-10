using Microsoft.EntityFrameworkCore;
using VkCrudProject.Context;
using VkCrudProject.Interfaces;
using VkCrudProject.Models;

namespace VkCrudProject.Repositories
{
    public class UserStateRepository : IUserStateRepository
    {
        private readonly UserContext _userContext;

        public UserStateRepository(UserContext userContext)
        {
            _userContext = userContext;
        }

        public async Task AddUserStateAsync(UserState userState)
        {
            await _userContext.UserStates.AddAsync(userState);
            await SaveChanges();
        }

        public async Task<UserState> GetUserStateByIdAsync(uint id)
        {
            return await _userContext.UserStates.FirstOrDefaultAsync(userState_ => userState_.Id == id);

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
