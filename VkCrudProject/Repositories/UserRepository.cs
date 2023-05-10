using Microsoft.EntityFrameworkCore;
using VkCrudProject.Context;
using VkCrudProject.Interfaces;
using VkCrudProject.Models;

namespace VkCrudProject.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly UserContext _userContext;
        private readonly IUserStateRepository _userStateRepository;
        private readonly IUserGroupRepository _userGroupRepository;
        public UserRepository(UserContext userContext, IUserStateRepository userStateRepository, IUserGroupRepository userGroupRepository)
        {
            _userContext = userContext;
            _userStateRepository = userStateRepository;
            _userGroupRepository = userGroupRepository;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userContext.Users
                .Include(user => user.GroupOfUser)
                .Include(user => user.StateOfUser)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> GetUsersAsync(UserParameters userParameters)
        {
            return await _userContext.Users
                .Include(user => user.GroupOfUser)
                .Include(user => user.StateOfUser)
                .Skip((userParameters.PageNumber - 1) * userParameters.PageSize)
                .Take(userParameters.PageSize)
                .ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(uint id)
        {
            return await _userContext.Users.Include(user => user.GroupOfUser).Include(user => user.StateOfUser).FirstOrDefaultAsync(user => user.Id == id);
        }

        public async Task<User> GetUserByLoginAsync(string login)
        {
            return await _userContext.Users.Include(user => user.GroupOfUser).Include(user => user.StateOfUser).FirstOrDefaultAsync(user => user.Login == login);
        }

        public async Task RegisterUserAsync(User user)
        {
            _userContext.Users.Add(user);
            await SaveChanges();
        }

        public async Task<bool> BlockUserAsync(uint id)
        {
            var userToRemove = await GetUserByIdAsync(id);

            
            if (userToRemove != null)
            {
                // block id == 2 because of data seeding in ModelBuilderExtension
                userToRemove.StateId = (await _userStateRepository.GetUserStateByIdAsync(2)).Id;
                await SaveChanges();
                return true;
            }

            return false;
        }

        public async Task<bool> ActivateUserAsync(uint id)
        {
            var userToActivate = await GetUserByIdAsync(id);

            if (userToActivate != null)
            {
                // active id == 1 because of data seeding in ModelBuilderExtension
                userToActivate.StateId = (await _userStateRepository.GetUserStateByIdAsync(1)).Id;
                await SaveChanges();
                return true;
            }

            return false;
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
