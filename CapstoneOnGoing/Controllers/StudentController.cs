using CapstoneOnGoing.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Net;

namespace CapstoneOnGoing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly IStudentService _studentService;
        private readonly ILogger _logger;

        public StudentController(IStudentService studentService, ILogger logger)
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

        [HttpGet]
        public IActionResult GetStudentById(Guid studentId)
        {
            Student student = _studentService.GetStudentById(studentId);
            return Ok(student);
        }

        [HttpPost]
        public IActionResult CreateStudent(Student student)
        {
            //if student is existed, not create and return error
            bool isExisted = _studentService.GetStudentById(student.Id) != null;
            if (isExisted)
            {
                _logger.Warn($"{nameof(CreateStudent)} in {nameof(StudentController)} : Student Existed with {student.Id}");
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
                _logger.Error($"{nameof(UpdateStudent)} in {nameof(StudentController)}: Student with {student.Id} is not existed in database");
                return BadRequest("Student is not existed to update");
            }
        }
    }
}
