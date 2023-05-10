using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace VkCrudProject.Models
{
    public class UserGroup
    {
        [Key]
        public uint Id { get; set; }

        public UserRole Role { get; set; } 
        public string Description { get; set; }

    }

    public enum UserRole
    {
        Admin,
        User
    }
}
