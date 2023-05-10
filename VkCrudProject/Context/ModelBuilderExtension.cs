using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Diagnostics.Metrics;
using System.Security.Cryptography;
using System.Text;
using VkCrudProject.Models;

namespace VkCrudProject.Context
{
    public static class ModelBuilderExtension
    {
        // data seeding


        public static void Seed(this ModelBuilder modelBuilder, User admin)
        {
            modelBuilder.Entity<UserGroup>().HasData(
                    new UserGroup
                    {
                        Id = 1,
                        Role = UserRole.Admin,
                        Description = "Administrator: moderating and maintaining service"
                    },
                    new UserGroup
                    {
                        Id = 2,
                        Role = UserRole.User,
                        Description = "User"
                    }
                );

            modelBuilder.Entity<UserState>().HasData(
                   new UserState
                   {
                       Id = 1,
                       Status = UserStatus.Active,
                       Description = "Active user"
                   },
                   new UserState
                   {
                       Id = 2,
                       Status = UserStatus.Blocked,
                       Description = "Blocked user"
                   }
               );

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = admin.Id,
                    Login = admin.Login,
                    Password = admin.Password,
                    CreatedAt = DateTimeOffset.UtcNow,
                    GroupId = 1,
                    StateId = 1
                }
                );
        }
    }
}
