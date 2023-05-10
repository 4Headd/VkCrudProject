using VkCrudProject.Models;

namespace VkCrudProject.Interfaces
{
    public interface IUserStateRepository : IDisposable
    {
        Task AddUserStateAsync(UserState userState);
        Task<UserState> GetUserStateByIdAsync(uint id);
        Task<bool> SaveChanges();

    }
}
