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

        [Authorize(Roles = "ADMIN")]
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

        [Authorize(Roles = "ADMIN")]
        [HttpGet("{id}")]
        public IActionResult GetLecturerById(Guid id)
        {
            User lecturer = _lecturerService.GetLecturerById(id);
            if (lecturer.Lecturer != null)
            {
                GetLecturerResponse lecturerResponse = _mapper.Map<GetLecturerResponse>(lecturer);
                return Ok(lecturerResponse);
            } else
            {
                return NotFound($"Cannot found lecturer with {id}");
            }
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public IActionResult CreateLecturer([FromBody] LecturerResquest lecturer)
        {
            bool isSuccessful = _userService.CreateNewLectuer(lecturer);
            GenericResponse response;
            if (isSuccessful)
            {
                response = new GenericResponse();
                response.HttpStatus = 201;
                response.Message = "Lecturer Created";
                response.TimeStamp = DateTime.Now;
                return CreatedAtAction(nameof(CreateLecturer), new { response });
            }
            else
            {
                _logger.LogWarn($"Controller: {nameof(UserController)},Method: {nameof(CreateLecturer)}, The user is exist");
                return BadRequest();
            }
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPut("{id}")]
        public IActionResult UpdateLecturer([FromBody] UpdateLecturerRequest lecturerUpdateRequest)
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
                return BadRequest($"User is not existed");
            }
        }
    }
}
