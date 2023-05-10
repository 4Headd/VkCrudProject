using System.ComponentModel.DataAnnotations;

namespace VkCrudProject.Models
{
    public class UserState
    {
        [Key]
        public uint Id { get; set; }

        public UserStatus Status { get; set; }

        public string Description { get; set; }
    }

    public enum UserStatus
    {
        Active,
        Blocked
    }
}
