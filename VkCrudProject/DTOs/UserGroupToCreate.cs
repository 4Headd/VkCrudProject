using VkCrudProject.Models;

namespace VkCrudProject.DTOs
{
    public class UserGroupToCreate
    {
        public uint Id { get; set; }
        public UserRole Role { get; set; }
    }
}
