using CapstoneOnGoing.Logger;
using CapstoneOnGoing.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Net;

namespace CapstoneOnGoing.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly ILoggerManager _logger;

        public StudentController(IStudentService studentService, ILoggerManager logger)
        {
            _studentService = studentService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetStudent()
        {
            IEnumerable<Student> students = _studentService.GetAllStudents();
            return Ok(students);
        }

        [HttpGet("{id}")]
        public IActionResult GetStudentById([FromQuery]Guid id)
        {
            Student student = _studentService.GetStudentById(id);
            return Ok(student);
        }

        [HttpPost]
        public IActionResult CreateStudent(Student student)
        {
            //if student is existed, not create and return error
            bool isExisted = _studentService.GetStudentById(student.Id) != null;
            if (isExisted)
            {
                _logger.LogWarn($"{nameof(CreateStudent)} in {nameof(StudentController)} : Student Existed with {student.Id}");
                return Conflict($"{student.Id} is existed");
            }
            else
            {
                _studentService.CreateStudent(student);
                return CreatedAtAction(nameof(CreateStudent), new {student.Id});
            }
        }

        [HttpPut]
        public IActionResult UpdateStudent(Student student)
        {
            //if student is exist, Update student, if not return error
            bool isExist = _studentService.GetStudentById(student.Id) != null;
            if (isExist)
            {
                _studentService.UpdateStudent(student);
                return CreatedAtAction(nameof(UpdateStudent), $"{student.ToString()} is updated");
            } else
            {
                _logger.LogError($"{nameof(UpdateStudent)} in {nameof(StudentController)}: Student with {student.Id} is not existed in database");
                return BadRequest("Student is not existed to update");
            }
        }
    }
}
