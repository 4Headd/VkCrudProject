using AutoMapper;
using Libraries.Authentication.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using VkCrudProject.Context;
using VkCrudProject.DTOs;
using VkCrudProject.Interfaces;
using VkCrudProject.Models;

namespace VkCrudProject.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserGroupRepository _userGroupRepository;
        private readonly IUserStateRepository _userStateRepository;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository, IUserGroupRepository userGroupRepository, IUserStateRepository userStateRepository, IMapper mapper, IUserService userService)
        {
            _userRepository = userRepository;
            _userGroupRepository = userGroupRepository;
            _userStateRepository = userStateRepository;
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserToRead>>> GetAllUsers()
        {
            var allUsers = await _userRepository.GetAllUsersAsync();
            return Ok(_mapper.Map<IEnumerable<UserToRead>>(allUsers));
        }

        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<UserToRead>>> GetUsers([FromQuery] UserParameters userParameters)
        {
            if (userParameters.PageNumber <= 0  || userParameters.PageSize <= 0)
            {
                // return Redirect("/api/users/get?pagenumber=1&pagesize=10");
                return BadRequest();
            }
            var users = await _userRepository.GetUsersAsync(userParameters);
            return Ok(_mapper.Map<IEnumerable<UserToRead>>(users));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(uint id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            if (user != null)
            {
                return Ok(_mapper.Map<UserToRead>(user));
            }

            return NotFound();
        }

        [HttpGet("login/{login}")]
        public async Task<ActionResult<User>> GetUserByLogin(string login)
        {
            var user = await _userRepository.GetUserByLoginAsync(login);

            if (user != null)
            {
                return Ok(_mapper.Map<UserToRead>(user));
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> Registration(UserToCreate user, uint idOfGroup)
        {
            try
            {
                await _userService.RegistrationAsync(user, idOfGroup);
                return Ok();
            }
            catch (Exception e)
            {
                return UnprocessableEntity(e.Message);
            }
        }

        [HttpPut]
        [BasicAuthorization]
        public async Task<ActionResult> BlockUser(uint id)
        {
            if (await _userRepository.BlockUserAsync(id))
            {
                return NoContent();
            }
            return NotFound();
        }
    }
}
