using AutoMapper;
using VkCrudProject.DTOs;
using VkCrudProject.Models;

namespace VkCrudProject.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserToRead>();
            CreateMap<UserToCreate, User>();
        }
    }
}
