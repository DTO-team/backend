using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CapstoneOnGoing.Enums;
using CapstoneOnGoing.Filter;
using CapstoneOnGoing.Helper;
using CapstoneOnGoing.Logger;
using CapstoneOnGoing.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Models.Dtos;
using Models.Models;
using Models.Response;
using Models.Response.GetAllUserResponse;

namespace CapstoneOnGoing.Controllers
{
	[Route("api/v1/users")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly IUserService _userService;
        private readonly ILecturerService _lecturerService;
        private readonly IStudentService _studentService;
		private readonly ILoggerManager _logger;
        private readonly IUriService _uriService;

		public UserController(IMapper mapper, ILoggerManager logger, IUserService userService, IUriService uriService, ILecturerService lecturerService, IStudentService studentService)
		{
			_mapper = mapper;
			_userService = userService;
			_logger = logger;
			_uriService = uriService;
            _studentService = studentService;
            _lecturerService = lecturerService;
        }

		// [Authorize(Roles = "ADMIN,STUDENT,LECTURER")]
		[HttpGet]
		public IActionResult GetAllUser([FromQuery] PaginationFilter paginationFilter)
		{
			string route = Request.Path.Value;
			PaginationFilter validFilter;
			if (string.IsNullOrEmpty(paginationFilter.SearchString) ||
			    string.IsNullOrWhiteSpace(paginationFilter.SearchString))
			{
				validFilter =
					new PaginationFilter(String.Empty, paginationFilter.PageNumber, paginationFilter.PageSize);
			}
			else
			{
				validFilter =
					new PaginationFilter(paginationFilter.SearchString.Trim(), paginationFilter.PageNumber, paginationFilter.PageSize);
			}
			IEnumerable<User> users = _userService.GetAllUsers(paginationFilter.SearchString, paginationFilter.PageNumber, paginationFilter.PageSize, out int totalRecords);
			if (users != null)
			{
				IEnumerable<UserInAdminDTO> usersInAdminDTO = _mapper.Map<IEnumerable<UserInAdminDTO>>(users);
                List<UserResponse> userResponses = new List<UserResponse>();

				Array.ForEach(usersInAdminDTO.ToArray(), user =>
                {
                    string userRole = user.Role;
                    if (userRole.Equals(RoleEnum.Lecturer.ToString().ToUpper()))
                    {
                        User lecturer = _lecturerService.GetLecturerById(user.Id);
                        LecturerUserResponse lecturerUser = _mapper.Map<LecturerUserResponse>(lecturer);
                        
                        userResponses.Add(lecturerUser);
                    }

                    if (userRole.Equals(RoleEnum.Student.ToString().ToUpper()))
                    {
                        User student = _studentService.GetStudentById(user.Id);
                        StudentUserResponse studentUser = _mapper.Map<StudentUserResponse>(student);
                        userResponses.Add(studentUser);
                    }
                });

				// var pagedResponse =
				// 	PaginationHelper<UserInAdminDTO>.CreatePagedResponse(usersInAdminDTO,validFilter,totalRecords,_uriService,route);
                 var pagedResponse =
                PaginationHelper<UserResponse>.CreatePagedResponse(userResponses, validFilter, totalRecords, _uriService, route);
                return Ok(pagedResponse);
			}
			return Ok(new List<UserInAdminDTO>());
		}

        [Authorize(Roles = "ADMIN,STUDENT,LECTURER")]
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
        [ProducesResponseType(typeof(StudentResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(GenericResponse), StatusCodes.Status400BadRequest)]
		public IActionResult CreateNewUser([FromBody] CreateNewUserRequest newUserData)
        {
            User user = _userService.GetUserWithRoleByEmail(newUserData.Email);
            User createdUser;

			if (user is null)
            {
                //User activated immediately at the time user is created
                if (newUserData.StatusId != 1)
                {
                    newUserData.StatusId = 1;
                }
                _userService.CreateUser(newUserData);
			}
            else
            {
                throw new BadHttpRequestException($"User with {newUserData.Email} is existed");
            }
            createdUser = _userService.GetUserWithRoleByEmail(newUserData.Email);
            StudentResponse createdUserResponse = _mapper.Map<StudentResponse>(createdUser);
            return CreatedAtAction("createNewUser", createdUserResponse);
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
