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
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IStudentService _studentService;
        private readonly ILecturerService _lecturerService;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public AdminController(IUserService userService, IStudentService studentService, ILecturerService lecturerService, ILoggerManager logger, IMapper mapper)
        {
            _userService = userService;
            _studentService = studentService;
            _lecturerService = lecturerService;
            _logger = logger;
            _mapper = mapper;
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("users")]
        public IActionResult GetAllUser()
        {
            IEnumerable<User> users = _userService.GetAllUsers();

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
                return BadRequest($"User with {id} is not exist");
            }
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost("user")]
        public IActionResult CreateNewUser([FromBody] CreateNewUserDTO createNewUserDTO)
        {
            User user = _userService.GetUserByEmail(createNewUserDTO.Email);
            if (user != null)
            {
                return Conflict(new ErrorDetails { StatusCode = (int)HttpStatusCode.Conflict, Message = $"{createNewUserDTO.Email} is already exist" });
            }
            else
            {
                _userService.CreateUser(createNewUserDTO);
                return CreatedAtAction(nameof(CreateNewUser), createNewUserDTO.Email);
            }
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPut("user")]
        public IActionResult UpdateUser([FromBody] UpdateUserInAdminDTO userInAdminToUpdate)
        {
            User user = _userService.GetUserById(userInAdminToUpdate.Id);
            if(user != null)
            {
                _userService.UpdateUser(user, userInAdminToUpdate.Role, userInAdminToUpdate.StatusId);
                return Ok(userInAdminToUpdate);
            } else
            {
                return BadRequest($"User with {userInAdminToUpdate.Id} is not existed");
            }
        }
    }
}
