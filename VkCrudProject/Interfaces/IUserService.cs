using VkCrudProject.DTOs;

namespace VkCrudProject.Interfaces
{
    public interface IUserService
    {
        Task RegistrationAsync(UserToCreate user, uint idOfGroup);
    }
}
