using VkCrudProject.Models;

namespace VkCrudProject.Interfaces
{
    public interface IUserRepository : IDisposable
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<IEnumerable<User>> GetUsersAsync(UserParameters userParameters);
        Task<User> GetUserByIdAsync(uint id);
        Task<User> GetUserByLoginAsync(string name);
        Task RegisterUserAsync(User user);
        Task<bool> BlockUserAsync(uint id);

        Task<bool> ActivateUserAsync(uint id);
        Task<bool> SaveChanges();
    }
}
