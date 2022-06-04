using CapstoneOnGoing.Logger;
using CapstoneOnGoing.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneOnGoing.Controllers
{
    [Route("api/v1/lecturers")]
    [ApiController]
    public class LecturerController : ControllerBase
    {
        private readonly ILecturerService _lecturerService;
        private readonly ILoggerManager _logger;

        public LecturerController(ILecturerService lecturerService, ILoggerManager logger)
        {
            _lecturerService = lecturerService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetAllLecturers()
        {
            IEnumerable<Lecturer> lecturers = _lecturerService.GetAllLecturers();
            return Ok(lecturers);
        }

        [HttpGet("{id}")]
        public IActionResult GetLecturerById(Guid id)
        {
            Lecturer lecturer = _lecturerService.GetLecturerById(id);
            return Ok(lecturer);
        }

        [HttpPost]
        public IActionResult CreateLecturer(Lecturer lecturer)
        {
            _lecturerService.CreateLecturer(lecturer);
            return CreatedAtAction(nameof(CreateLecturer), new { lecturer.Id });
        }

        [HttpPut]
        public IActionResult UpdateLecturer(Lecturer lecturer)
        {
            //if student is exist, Update student, if not return error
            bool isExist = _lecturerService.GetLecturerById(lecturer.Id) != null;
            if (isExist)
            {
                _lecturerService.UpdateLecturer(lecturer);
                return Ok(lecturer.Id);
            }
            else
            {
                _logger.LogError($"{nameof(UpdateLecturer)} in {nameof(LecturerController)}: Lecturer with {lecturer.Id} is not existed in database");
                return BadRequest("Student is not existed to update");
            }
        }
    }
}
