using AutoMapper;
using VkCrudProject.DTOs;
using VkCrudProject.Interfaces;
using VkCrudProject.Models;
using VkCrudProject.Repositories;

namespace VkCrudProject.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserGroupRepository _userGroupRepository;
        private readonly IUserStateRepository _userStateRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IUserGroupRepository userGroupRepository, IUserStateRepository userStateRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _userGroupRepository = userGroupRepository;
            _userStateRepository = userStateRepository;
            _mapper = mapper;
        }

        public async Task RegistrationAsync(UserToCreate user, uint idOfGroup)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            if ((await _userRepository.GetUserByLoginAsync(user.Login)) != null)
            {
                throw new ArgumentException("Login already exists");
            }

            User newUser = new User();
            _mapper.Map(user, newUser);
            newUser.CreatedAt = DateTimeOffset.Now;

            var userGroup = _userGroupRepository.GetUserGroupByIdAsync(idOfGroup).Result ?? throw new ArgumentException("No such group in database");
            if (userGroup.Id == 1)
            {
                throw new ArgumentException("Cant create more Administrators");
            }
            newUser.GroupId = userGroup.Id;


            var userState = await _userStateRepository.GetUserStateByIdAsync(2);
            newUser.StateId = userState.Id;


            await _userRepository.RegisterUserAsync(newUser);
            System.Threading.Thread.Sleep(5000);
            await _userRepository.ActivateUserAsync(newUser.Id);
        }
    }

}
