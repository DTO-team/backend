﻿using CapstoneOnGoing.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CapstoneOnGoing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LecturerController : ControllerBase
    {
        private readonly ILecturerService _lecturerService;
        private readonly ILogger _logger;

        public LecturerController(ILecturerService lecturerService, ILogger logger)
        {
            _lecturerService = lecturerService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetAllLecturer()
        {
            IEnumerable<Lecturer> lecturers =_lecturerService.GetAllLecturers();
            return Ok(lecturers);
        }

        [HttpGet]
        public IActionResult GetLecturerById(Guid studentId)
        {
            Lecturer lecturer = _lecturerService.GetLecturerById(studentId);
            return Ok(lecturer);
        }

        [HttpPost]
        public IActionResult CreateLecturer(Lecturer lecturer)
        {
            bool isExisted = _lecturerService.GetLecturerById(lecturer.Id) != null;
            if (isExisted)
            {
                _logger.Warn($"{nameof(CreateLecturer)} in {nameof(LecturerController)}: Lecturer with {lecturer.Id} is existed");
                return BadRequest("lecturer is existed");
            } else
            {
                _lecturerService.CreateLecturer(lecturer);
                return CreatedAtAction(nameof(CreateLecturer), new { lecturer.Id });
            }
        }

        [HttpPut]
        public IActionResult UpdateLecturer(Lecturer lecturer)
        {
            //if student is exist, Update student, if not return error
            bool isExist = _lecturerService.GetLecturerById(lecturer.Id) != null;
            if (isExist)
            {
                _lecturerService.UpdateLecturer(lecturer);
                return CreatedAtAction(nameof(UpdateLecturer), $"{lecturer} is updated");
            }
            else
            {
                _logger.Error($"{nameof(UpdateLecturer)} in {nameof(LecturerController)}: Lecturer with {lecturer.Id} is not existed in database");
                return BadRequest("Student is not existed to update");
            }
        }
    }
}
