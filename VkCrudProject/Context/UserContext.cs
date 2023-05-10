using Microsoft.EntityFrameworkCore;
using VkCrudProject.Models;

namespace VkCrudProject.Context
{
    public class UserContext : DbContext
    {
        private readonly IConfiguration configuration;
        public UserContext(DbContextOptions<UserContext> options, IConfiguration configuration) : base(options)
        {
            this.configuration = configuration;
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var section = configuration.GetSection("BaseUser");
            User admin = new User
            {
                Id = section.GetValue<uint>("Id"),
                Login = section.GetValue<string>("Login"),
                Password = section.GetValue<string>("Password")
            };

            modelBuilder.Seed(admin);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<UserState> UserStates { get; set; }

    }
}
