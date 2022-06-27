using AutoMapper;
using CapstoneOnGoing.Logger;
using CapstoneOnGoing.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Dtos;
using Models.Models;
using Models.Response;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace CapstoneOnGoing.Controllers
{
    [Route("api/v1/lecturers")]
    [ApiController]
    public class LecturerController : ControllerBase
    {
        private readonly ILecturerService _lecturerService;
        private readonly IUserService _userService;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public LecturerController(ILecturerService lecturerService, IUserService userService, ILoggerManager logger, IMapper mapper)
        {
            _lecturerService = lecturerService;
            _userService = userService;
            _logger = logger;
            _mapper = mapper;
        }

        [Authorize(Roles = "ADMIN,LECTURER,STUDENT")]
        [HttpGet]
        public IActionResult GetAllLecturers([FromQuery] int page, [FromQuery] int limit)
        {
            IEnumerable<GetLecturerResponse> lecturers;
            if (page == 0 || limit == 0 || page < 0 || limit < 0)
            {
                lecturers = _mapper.Map<IEnumerable<GetLecturerResponse>>(_lecturerService.GetAllLecturers(1, 10));
            }
            else
            {
                lecturers = _mapper.Map<IEnumerable<GetLecturerResponse>>(_lecturerService.GetAllLecturers(page, limit));
            }
            return Ok(lecturers);
        }

        [Authorize(Roles = "ADMIN,LECTURER,STUDENT")]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetLecturerResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenericResponse), StatusCodes.Status400BadRequest)]
        public IActionResult GetLecturerById(Guid id)
        {
            User lecturer = _lecturerService.GetLecturerById(id);
            if (lecturer.Lecturer != null)
            {
                GetLecturerResponse lecturerResponse = _mapper.Map<GetLecturerResponse>(lecturer);
                return Ok(lecturerResponse);
            } else
            {
                return NotFound(new GenericResponse(){HttpStatus = 400, Message =  $"Cannot found lecturer with {id}", TimeStamp = DateTime.Now});
            }
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        [ProducesResponseType(typeof(GetLecturerResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(GenericResponse), StatusCodes.Status400BadRequest)]
        public IActionResult CreateLecturer([FromBody] LecturerResquest lecturer)
        {
            bool isSuccessful = _userService.CreateNewLectuer(lecturer);
            if (isSuccessful)
            {
                User lecturerUser = _lecturerService.GetLecturerByEmail(lecturer.Email);
                GetLecturerResponse lecturerResponse = _mapper.Map<GetLecturerResponse>(lecturerUser);
                return CreatedAtAction(nameof(CreateLecturer), lecturerResponse);
            }
            else
            {
                _logger.LogWarn($"Controller: {nameof(UserController)},Method: {nameof(CreateLecturer)}, The user is exist");
                return BadRequest(new GenericResponse()
                {
                    HttpStatus = 400,
                    Message = "Create Lecturer Failed !",
                    TimeStamp = DateTime.Now
                });
            }
        }

        [Authorize(Roles = "ADMIN,LECTURER")]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(GetLecturerResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenericResponse), StatusCodes.Status400BadRequest)]
        public IActionResult UpdateLecturer([FromBody] UpdateLecturerRequest lecturerUpdateRequest)
        {
            string userEmail = HttpContext.User.FindFirstValue(ClaimTypes.Email);
            User userByEmail = _userService.GetUserWithRoleByEmail(userEmail);
            if (userByEmail.Id.Equals(lecturerUpdateRequest.Id))
            {
                User updateUser = _lecturerService.UpdateLecturer(_mapper.Map<User>(lecturerUpdateRequest));
                if (updateUser != null)
                {
                    User lecturer = _lecturerService.GetLecturerById(updateUser.Id);
                    return Ok(_mapper.Map<GetLecturerResponse>(lecturer));
                }
                else
                {
                    _logger.LogWarn($"Controller: {nameof(UserController)},Method: {nameof(UpdateLecturer)}, The user {lecturerUpdateRequest.Id} do not exist");
                    return BadRequest(new GenericResponse() { HttpStatus = 400, Message = $"User is not existed", TimeStamp = DateTime.Now });
                }
            }
            else
            {
                return BadRequest("Cannot update profile of another lecturer! Only update yourself!");
            }
        }
    }
}
