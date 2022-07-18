using AutoMapper;
using CapstoneOnGoing.Logger;
using CapstoneOnGoing.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Dtos;
using Models.Models;
using Models.Request;
using Models.Response;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace CapstoneOnGoing.Controllers
{
    [Route("api/v1/students")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly ILoggerManager _logger;
        private readonly ITeamService _teamService;
        private readonly IUserService _userService;
        private readonly ITeamStudentService _teamStudentService;
        private readonly IMapper _mapper;

        public StudentController(IStudentService studentService, ILoggerManager logger, ITeamService teamService, IUserService userService, ITeamStudentService teamStudentService, IMapper mapper)
        {
            _studentService = studentService;
            _logger = logger;
            _teamService = teamService;
            _userService = userService;
            _teamStudentService = teamStudentService;
            _mapper = mapper;
        }

        // [Authorize(Roles = "ADMIN,LECTURER,STUDENT")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<StudentResponse>), StatusCodes.Status200OK)]
        public IActionResult GetAllStudents()
        {
            IEnumerable<StudentResponse> students =_mapper.Map<IEnumerable<StudentResponse>>(_studentService.GetAllStudents());
            return Ok(students);
        }

        [Authorize(Roles = "ADMIN,LECTURER,STUDENT")]
        [HttpGet("{id}/teams")]
        [ProducesResponseType(typeof(GetTeamDetailResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenericResponse), StatusCodes.Status400BadRequest)]
        public IActionResult GetStudentTeamByStudentId(Guid id)
        {
            TeamStudent teamStudent = _teamStudentService.GetTeamStudentByStudentId(id);
            if (teamStudent is not null)
            {
                GetTeamDetailResponse teamDetailResponse = _teamService.GetTeamDetail(teamStudent.TeamId);
                return Ok(teamDetailResponse);
            }
            else
            {
                return BadRequest(new GenericResponse()
                { HttpStatus = 400, Message = "Get student team detail failed. Student is not in any team!", TimeStamp = DateTime.Now });
            }
        }

        [Authorize(Roles = "ADMIN,LECTURER,STUDENT")]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(StudentResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenericResponse), StatusCodes.Status404NotFound)]
        public IActionResult GetStudentById(Guid id)
        {
            User student = _studentService.GetStudentById(id);
            if (student.Student != null)
            {

                StudentResponse studentDTO = _mapper.Map<StudentResponse>(student);
                if (!studentDTO.TeamId.Equals(Guid.Empty))
                {
                    GetTeamDetailResponse teamDetailResponse = _teamService.GetTeamDetail(studentDTO.TeamId);
                    if (teamDetailResponse is not null)
                    {
                        studentDTO.TeamDetail = teamDetailResponse;
                    }
                }

                return Ok(studentDTO);
            }
            else
            {
                return NotFound(new GenericResponse(){HttpStatus = 404, Message = $"Cannot found student with {id}" , TimeStamp = DateTime.Now});
            }
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        [ProducesResponseType(typeof(StudentResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(GenericResponse), StatusCodes.Status400BadRequest)]
        public IActionResult CreateStudent([FromBody] StudentRequest student)
        {
            MailAddress email = new MailAddress(student.Email);
            string domain = email.Host;

            if (domain != "fpt.edu.vn")
            {
                return BadRequest("Wrong email format");
            }
            else
            {
                bool isSuccessful = _userService.CreateNewStudent(student);
                if (isSuccessful)
                {
                    User createdStudent = _studentService.GetStudentByEmail(student.Email);
                    StudentResponse createdStudentResponse = _mapper.Map<StudentResponse>(createdStudent);
                    return CreatedAtAction(nameof(CreateStudent), createdStudentResponse);
                }
                else
                {
                    return BadRequest(
                        new GenericResponse()
                        {
                            HttpStatus = 400,
                            Message = "Create Student failed",
                            TimeStamp = DateTime.Now
                        }
                        );
                }
            }
        }

        [Authorize(Roles = "ADMIN,LECTURER,STUDENT")]
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(StudentUpdateResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenericResponse), StatusCodes.Status400BadRequest)]
        public IActionResult UpdateStudent(Guid id,UpdateStudentRequest student)
        {
            string userEmail = HttpContext.User.FindFirstValue(ClaimTypes.Email);
            User userByEmail = _userService.GetUserWithRoleByEmail(userEmail);
            if (userByEmail.Id.Equals(id))
            {
                //if student is exist, Update student, if not return error
                User updatedUser = _studentService.UpdateStudent(id, student);
                User updatedUserDetail = _userService.GetUserWithRoleByEmail(updatedUser.Email);
                if (updatedUserDetail != null)
                {
                    return Ok(_mapper.Map<StudentUpdateResponse>(updatedUserDetail));
                }
                else
                {
                    _logger.LogError($"{nameof(UpdateStudent)} in {nameof(StudentController)}: Student with {id} is not existed in database");
                    return BadRequest(new GenericResponse() { HttpStatus = 400, Message = "Student is not existed to update", TimeStamp = DateTime.Now });
                }
            }
            else
            {
                return BadRequest("Cannot update profile of another student! Only update yourself!");
            }
        }
    }
}
