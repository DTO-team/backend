using AutoMapper;
using CapstoneOnGoing.Logger;
using CapstoneOnGoing.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
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

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        public IActionResult CreateStudent(Student student)
        {
            _studentService.CreateStudent(student);
            return CreatedAtAction(nameof(CreateStudent), new { student.Id });
        }

	    [Authorize(Roles = "ADMIN,LECTURER")]
        [HttpPut]
        public IActionResult UpdateStudent(Student student)
        {
            //if student is exist, Update student, if not return error
            bool isExist = _studentService.GetStudentById(student.Id) != null;
            if (isExist)
            {
                _studentService.UpdateStudent(student);
                return Ok(student.Id);

            } else
            {
                _logger.LogError($"{nameof(UpdateStudent)} in {nameof(StudentController)}: Student with {student.Id} is not existed in database");
                return BadRequest("Student is not existed to update");
            }
        }
    }
}
