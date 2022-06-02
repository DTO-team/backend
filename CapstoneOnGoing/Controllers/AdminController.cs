using CapstoneOnGoing.Logger;
using CapstoneOnGoing.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Dtos;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CapstoneOnGoing.Controllers
{
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IStudentService _studentService;
        private readonly ILecturerService _lecturerService;
        private ILoggerManager _logger;

        public AdminController(IStudentService studentService, ILecturerService lecturerService, ILoggerManager logger)
        {
            _studentService = studentService;
            _lecturerService = lecturerService;
            _logger = logger;
        }
        
        [Authorize(Roles = "ADMIN")]
        [HttpGet("users")]
        public IActionResult GetAllUser()
        {
            IEnumerable<User> users = _userService.GetAllUsers();
            if (users.Count() > 0)
            {
                return Ok(users);
            }
            else
            {
                _logger.LogError($"{nameof(GetAllUser)} in {nameof(AdminController)}: Cannot load data from database");
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [Authorize(Roles = "ADMIN")]
        [HttpGet("{id}")]
        public IActionResult GetUserById(Guid id)
        {
            User user = _userService.GetUserById(id);
            if (user != null)
            {
                return Ok(user);
            } else
            {
                return BadRequest($"User with {id} is not exist");
            }
        }

        [HttpPost("user")]
        public IActionResult CreateNewUser([FromBody] CreateNewUserDTO createNewUserDTO)
        {
            User user = _userService.GetUserByEmail(createNewUserDTO.Email);
            if(user != null)
            {
                return Conflict(new ErrorDetails { StatusCode = (int)HttpStatusCode.Conflict, Message = $"{createNewUserDTO.Email} is already exist" }); 
            }
            return CreatedAtAction(nameof(CreateNewUser),createNewUserDTO.Email);
        }
    }
}
