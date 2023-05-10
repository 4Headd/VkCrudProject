using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VkCrudProject.Models;

namespace VkCrudProject.DTOs
{
    public class UserToRead
    {
        public uint Id { get; set; }


        public string Login { get; set; }

        public string Password { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        public uint GroupId { get; set; }

        [ForeignKey("GroupId")]
        public UserGroup GroupOfUser { get; set; }

        public uint StateId { get; set; }
        [ForeignKey("StateId")]
        public UserState StateOfUser { get; set; }
    }
}
