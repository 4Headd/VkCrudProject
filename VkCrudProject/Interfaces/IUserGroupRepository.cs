using VkCrudProject.Models;

namespace VkCrudProject.Interfaces
{
    public interface IUserGroupRepository : IDisposable
    {
        Task AddUserGroupAsync(UserGroup userGroup);
        Task<UserGroup> GetUserGroupByIdAsync(uint id);
        Task<bool> SaveChanges();
    }
}
