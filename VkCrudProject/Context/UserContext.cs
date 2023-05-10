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

            /*modelBuilder.Entity<User>(eb =>
            {
                eb.Property(u => u.Id);
                eb.Property(u => u.Login).IsRequired().HasMaxLength(20);
                eb.Property(u => u.Password).IsRequired().HasMaxLength(16);
                eb.Property(u => u.CreatedAt).IsRequired();
                eb.Property(u => u.GroupId);
                eb.Property(u => u.StateId);
            }
            );

            modelBuilder.Entity<UserGroup>(eb =>
            {
                eb.Property(uG => uG.Id);
                eb.Property(uG => uG.Role);
                eb.Property(uG => uG.Description);
            }
            );

            modelBuilder.Entity<UserState>(eb =>
            {
                eb.Property(uS => uS.Id);
                eb.Property(uS => uS.Status);
                eb.Property(uS => uS.Description);
            }
            );*/

            modelBuilder.Seed(admin);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<UserGroup> UserGroups { get; set; }
        public DbSet<UserState> UserStates { get; set; }

    }
}
