using AutoMapper;
using CapstoneOnGoing.Logger;
using CapstoneOnGoing.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Dtos;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Net;

namespace CapstoneOnGoing.Controllers
{
	[Route("api/v1/admin")]
	[ApiController]
	public class AdminController : ControllerBase
	{
		private readonly IUserService _userService;
		private readonly IStudentService _studentService;
		private readonly ILecturerService _lecturerService;
		private readonly ISemesterService _semesterService;
		private readonly ILoggerManager _logger;
		private readonly IMapper _mapper;

		public AdminController(IUserService userService, IStudentService studentService, ILecturerService lecturerService, ILoggerManager logger, IMapper mapper, ISemesterService semesterService)
		{
			_userService = userService;
			_studentService = studentService;
			_lecturerService = lecturerService;
			_logger = logger;
			_mapper = mapper;
			_semesterService = semesterService;
		}

		[Authorize(Roles = "ADMIN")]
		[HttpGet("users")]
		public IActionResult GetAllUser([FromQuery] string name, [FromQuery] int page, [FromQuery] int limit)
		{
			IEnumerable<User> users = _userService.GetAllUsers(name, page, limit);

			if (users != null)
			{
				IEnumerable<UserInAdminDTO> usersInAdminDTO = _mapper.Map<IEnumerable<UserInAdminDTO>>(users);
				return Ok(usersInAdminDTO);
			}
			else
			{
				return NotFound(new List<UserInAdminDTO>()); ;
			}
		}

		[Authorize(Roles = "ADMIN")]
		[HttpGet("users/{id}")]
		public IActionResult GetUserById(Guid id)
		{
			User user = _userService.GetUserById(id);
			if (user != null)
			{
				return Ok(user);
			}
			else
			{
				_logger.LogWarn($"Controller: {nameof(AdminController)},Method: {nameof(GetUserById)}, The user {id} do not exist");
				return BadRequest($"User is not exist");
			}
		}

		[Authorize(Roles = "ADMIN")]
		[HttpPost("users")]
		public IActionResult CreateNewUser([FromBody] CreateNewUserDTO createNewUserDTO)
		{
			User user = _userService.GetUserByEmail(createNewUserDTO.Email);
			if (user != null)
			{
				_logger.LogWarn($"Controller: {nameof(AdminController)},Method: {nameof(CreateNewUser)}, The user {createNewUserDTO.Email} is already exist");
				return Conflict(new ErrorDetails { StatusCode = (int)HttpStatusCode.Conflict, Message = $"{createNewUserDTO.Email} is already exist" });
			}
			else
			{
				_userService.CreateUser(createNewUserDTO);
				return CreatedAtAction(nameof(CreateNewUser), createNewUserDTO.Email);
			}
		}

		[Authorize(Roles = "ADMIN")]
		[HttpPut("users/{id}")]
		public IActionResult UpdateUser([FromBody] UpdateUserInAdminDTO userInAdminToUpdate)
		{
			User user = _userService.GetUserById(userInAdminToUpdate.Id);
			if (user != null)
			{
				_userService.UpdateUser(user, userInAdminToUpdate.Role);
				return Ok(user);
			}
			else
			{
				_logger.LogWarn($"Controller: {nameof(AdminController)},Method: {nameof(UpdateUser)}, The user {userInAdminToUpdate.Id} do not exist");
				return BadRequest($"User is not existed");
			}
		}

		[Authorize(Roles = "ADMIN")]
		[HttpGet("semesters")]
		public IActionResult GetAllSemester([FromQuery] int page, [FromQuery] int limit)
		{
			IEnumerable<Semester> semesters = _semesterService.GetAllSemesters(page, limit);
			IEnumerable<GetSemesterDTO> semestersDTO = _mapper.Map<IEnumerable<GetSemesterDTO>>(semesters);
			return Ok(semestersDTO);
		}

		[Authorize(Roles = "ADMIN")]
		[HttpPost("semesters")]
		public IActionResult CreateNewSemester([FromBody] CreateNewSemesterDTO newSemesterDTO)
		{
			Semester newSemester = _mapper.Map<Semester>(newSemesterDTO);
			newSemester = _semesterService.CreateNewSemester(newSemester);
			if (newSemester != null)
			{
				GetSemesterDTO result = _mapper.Map<GetSemesterDTO>(newSemester);
				return CreatedAtAction(nameof(CreateNewSemester), result);
			}
			else
			{
				_logger.LogWarn($"Controller: {nameof(AdminController)},Method: {nameof(CreateNewSemester)}, The {newSemesterDTO.Year} - {newSemesterDTO.Season} is already existed");
				return BadRequest($"Semester {newSemesterDTO.Year} - {newSemesterDTO.Season} is already existed");
			}
		}

		[Authorize(Roles = "ADMIN")]
		[HttpPut("semesters/{id}")]
		public IActionResult UpdateSemester([FromBody] UpdateSemesterDTO updateSemesterDTO)
		{
			Semester updatedSemester = _semesterService.GetSemesterById(updateSemesterDTO.Id);
			if (updatedSemester != null)
			{
				bool isSuccessful = _semesterService.UpdateSemester(updatedSemester,updateSemesterDTO);
				if (isSuccessful)
				{
					return Ok(updateSemesterDTO);
				}
				_logger.LogWarn($"Controller: {nameof(AdminController)},Method: {nameof(UpdateSemester)}, The {updatedSemester.Year} - {updatedSemester.Season} update failed");
				return BadRequest($"The semester {updatedSemester.Year} - {updatedSemester.Season} updated failed");
			}
			else
			{
				_logger.LogWarn($"Controller: {nameof(AdminController)},Method: {nameof(UpdateSemester)}, The {updateSemesterDTO.Id} is not existed");
				return BadRequest($"Semester does not exist");
			}
		}
	}
}
