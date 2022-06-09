using Microsoft.AspNetCore.Http;
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

        //[Authorize(Roles = "ADMIN")]
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

        //[Authorize(Roles = "ADMIN")]
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

        //[Authorize(Roles = "ADMIN")]
        [HttpPost]
		public IActionResult CreateNewUser([FromBody] CreateNewUserRequest newUserData)
		{
			User user = _userService.GetUserByEmail(newUserData.Email);
			if (user != null)
			{
				_logger.LogWarn($"Controller: {nameof(UserController)},Method: {nameof(CreateNewUser)}, The user {newUserData.Email} is already exist");
				return Conflict($"The user {newUserData.Email} is already existed");
			}
			else
			{
				//User activated immediately at the time user is created
				if (newUserData.StatusId != 1)
				{
					newUserData.StatusId = 1;
				}
				_userService.CreateUser(newUserData);
				return CreatedAtAction(nameof(CreateNewUser), newUserData.Email);
			}
		}

		//[Authorize(Roles = "ADMIN")]
		[HttpPut("{id}")]
		public IActionResult UpdateUser([FromBody] UpdateUserInAdminRequest userInAdminUpdateData)
		{
			//Get user from database base on userInAdminToUpdate id
			User user = _userService.GetUserById(userInAdminUpdateData.Id);

			if (user != null)
			{
				//Cannot update user when user is inactivated
				if (user.StatusId != 1)
				{
					return BadRequest("User is not activated to update");
				}

				//Auto change status id to 1 if user is activated and you want to update user 
				if (!string.IsNullOrEmpty(userInAdminUpdateData.Role) && userInAdminUpdateData.StatusId == 0)
				{
					userInAdminUpdateData.StatusId = 1;
				}
				_userService.UpdateUser(user, userInAdminUpdateData.Role, userInAdminUpdateData.StatusId);
				return Ok(userInAdminUpdateData);
			}
			else
			{
				_logger.LogWarn($"Controller: {nameof(UserController)},Method: {nameof(UpdateUser)}, The user {userInAdminUpdateData.Id} do not exist");
				return BadRequest($"User is not existed");
			}
		}

		//[Authorize(Roles = "ADMIN")]
		[HttpDelete]
		public IActionResult DeleteUserById(Guid userId)
        {
			bool isDeleted = _userService.DeleteUserById(userId);
			if(isDeleted)
            {
				return NoContent();
            } else
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
