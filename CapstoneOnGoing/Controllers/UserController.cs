using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using AutoMapper;
using CapstoneOnGoing.Logger;
using CapstoneOnGoing.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Models.Dtos;
using Models.Models;
using Models.Response;

namespace CapstoneOnGoing.Controllers
{
	[Route("api/v1/users")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly IUserService _userService;
		private readonly ILoggerManager _logger;

		public UserController(IMapper mapper, ILoggerManager logger, IUserService userService)
		{
			_mapper = mapper;
			_userService = userService;
			_logger = logger;
		}

        [Authorize(Roles = "ADMIN")]
        [HttpGet]
		public IActionResult GetAllUser([FromQuery] string username, [FromQuery] int page, [FromQuery] int limit)
		{

			IEnumerable<User> users = _userService.GetAllUsers(username, page, limit);

			if (users != null)
			{
				IEnumerable<UserInAdminDTO> usersInAdminDTO = _mapper.Map<IEnumerable<UserInAdminDTO>>(users);
				return Ok(usersInAdminDTO);
			}
			return Ok(new List<UserInAdminDTO>());
		}

        [Authorize(Roles = "ADMIN")]
        [HttpGet("{id}")]
		public IActionResult GetUserById(Guid id)
		{
			User user = _userService.GetUserById(id);
			if (user != null)
			{
				return Ok(_mapper.Map<UserByIdDTO>(user));
			}
			else
			{
				_logger.LogWarn($"Controller: {nameof(UserController)},Method: {nameof(GetUserById)}, The user {id} do not exist");
				return BadRequest($"User is not exist");
			}
		}

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public IActionResult CreateNewUser([FromBody] CreateNewUserRequest newUserData)
        {
            User user = _userService.GetUserWithRoleByEmail(newUserData.Email);
            //User activated immediately at the time user is created
            if (newUserData.StatusId != 1)
            {
                newUserData.StatusId = 1;
            }
            _userService.CreateUser(newUserData);
			return CreatedAtAction(nameof(CreateNewUser), newUserData.Email);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPut("{id}")]
        public IActionResult UpdateUser([FromBody] UpdateUserInAdminRequest userInAdminUpdateData)
        {
            User user = _userService.GetUserById(userInAdminUpdateData.Id);
            return Ok(user);
        }

        [Authorize(Roles = "ADMIN")]
        [HttpDelete("{id}")]
        public IActionResult DeleteUserById(Guid userId)
        {
            bool isDeleted = _userService.DeleteUserById(userId);
            if (isDeleted)
            {
                return NoContent();
            }
            else
            {
                GenericResponse response = new GenericResponse();
                response.HttpStatus = 409;
                response.Message = "Delete Failed";
                response.TimeStamp = DateTime.Now;
                return Conflict(response);
            }
        }
    }
}
