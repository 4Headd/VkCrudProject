using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VkCrudProject.Models;

namespace VkCrudProject.Models
{
    public class User
    {
        [Key]
        public uint Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Login { get; set; }

        [Required]
        [MinLength(8)]
        [MaxLength(16)]
        public string Password { get; set; }

        [Required]
        public DateTimeOffset CreatedAt { get; set; }

        public uint GroupId { get; set; }

        [ForeignKey("GroupId")]
        public UserGroup GroupOfUser { get; set; }

        public uint StateId { get; set; }
        [ForeignKey("StateId")]
        public UserState StateOfUser { get; set; }

    }
}
