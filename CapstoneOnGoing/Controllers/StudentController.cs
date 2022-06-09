using AutoMapper;
using CapstoneOnGoing.Logger;
using CapstoneOnGoing.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Dtos;
using Models.Models;
using Models.Request;
using Models.Response;
using System;
using System.Collections.Generic;
using System.Net;

namespace CapstoneOnGoing.Controllers
{
    [Route("api/v1/students")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly ILoggerManager _logger;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public StudentController(IStudentService studentService, ILoggerManager logger, IUserService userService, IMapper mapper)
        {
            _studentService = studentService;
            _logger = logger;
            _userService = userService;
            _mapper = mapper;
        }

        //[Authorize(Roles = "ADMIN,LECTURER")]
        [HttpGet]
        public IActionResult GetAllStudents([FromQuery] int page, [FromQuery] int limit)
        {
            IEnumerable<StudentResponse> students = _mapper.Map<IEnumerable<StudentResponse>>(_studentService.GetAllStudents(page,limit));
            return Ok(students);
        }
        
        //[Authorize(Roles = "ADMIN,LECTURER,STUDENT")]
        [HttpGet("{id}")]
        public IActionResult GetStudentById(Guid id)
        {
            User student = _studentService.GetStudentById(id);
            if (student.Student != null)
            {
                StudentResponse studentDTO = _mapper.Map<StudentResponse>(student);
                return Ok(studentDTO);
            }
            else
            {
                return NotFound($"Cannot found student with {id}");
            }
        }

        //[Authorize(Roles = "ADMIN")]
        [HttpPost]
        public IActionResult CreateStudent([FromBody] StudentRequest student)
        {
            if (!student.RoleId.Equals(3))
            {
                student.RoleId = 3;
            }
            if (!student.StatusId.Equals(1))
            {
                student.StatusId = 1;
            }
            GenericResponse response = _userService.CreateNewStudent(student);
            if (response.HttpStatus == 201)
            {
                return CreatedAtAction(nameof(CreateStudent), new { response.Message });
            }
            else
            {
                return BadRequest(response.Message);
            }
        }

        //[Authorize(Roles = "ADMIN,LECTURER")]
        [HttpPut]
        public IActionResult UpdateStudent(UpdateStudentRequest student)
        {
            //if student is exist, Update student, if not return error
            User userUpdate = _studentService.UpdateStudent(student);
            if (userUpdate != null)
            {
                return Ok(_mapper.Map<StudentUpdateResponseDTO>(userUpdate));

            } else
            {
                _logger.LogError($"{nameof(UpdateStudent)} in {nameof(StudentController)}: Student with {student.Id} is not existed in database");
                return BadRequest("Student is not existed to update");
            }
        }


    }
}
