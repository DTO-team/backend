﻿using AutoMapper;
using CapstoneOnGoing.Logger;
using CapstoneOnGoing.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Dtos;
using Models.Models;
using Models.Response;
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

        [HttpGet]
        public IActionResult GetAllLecturers([FromQuery] int page, [FromQuery] int limit)
        {
            IEnumerable<LecturerResponse> lecturers = _mapper.Map<IEnumerable<LecturerResponse>>(_lecturerService.GetAllLecturers(page, limit));
            return Ok(lecturers);
        }

        [HttpGet("{id}")]
        public IActionResult GetLecturerById(Guid id)
        {
            User lecturer = _lecturerService.GetLecturerById(id);
            if (lecturer.Lecturer != null)
            {
                LecturerResponse lecturerDTO = _mapper.Map<LecturerResponse>(lecturer);
                return Ok(lecturerDTO);
            } else
            {
                return NotFound($"Cannot found lecturer with {id}");
            }
        }

        [HttpPost]
        public IActionResult CreateLecturer([FromBody] LecturerResquest lecturer)
        {
            if (!lecturer.RoleId.Equals(2))
            {
                lecturer.RoleId = 2;
            }
            if (!lecturer.StatusId.Equals(1))
            {
                lecturer.StatusId = 1;
            }
            ////Create new user
            //CreateNewUserDTO newUserDTO = _mapper.Map<CreateNewUserDTO>(lecturer);
            //_userService.CreateUser(newUserDTO);

            ////Get id of new created user
            //Guid userId = _userService.GetUserIdByUserName(lecturer.UserName);
            //Guid departmentId = lecturer.DepartmentId;

            //Lecturer lecturerDTO = _mapper.Map<Lecturer>(lecturer);
            //_lecturerService.CreateLecturer(lecturerDTO, userId, departmentId);

            bool isSuccess = _userService.CreateNewLectuer(lecturer);

            if (isSuccess)
            {
                return CreatedAtAction(nameof(CreateLecturer), new { lecturer.UserName });
            }
            else
            {
                return BadRequest("Cannot Create lecturer");
            }


        }

        [HttpPut("{id}")]
        public IActionResult UpdateLecturer([FromBody] UpdateLecturerRequestDTO lecturerToUpdate)
        {
            if (!lecturerToUpdate.RoleId.Equals(2))
            {
                lecturerToUpdate.RoleId = 2;
            }

            User userUpdated = _lecturerService.UpdateLecturer(_mapper.Map<User>(lecturerToUpdate));
            if (userUpdated != null)
            {
                User lecturer = _lecturerService.GetLecturerById(userUpdated.Id);
                return Ok(_mapper.Map<LecturerResponse>(lecturer));
            }
            else
            {
                _logger.LogWarn($"Controller: {nameof(UserController)},Method: {nameof(UpdateLecturer)}, The user {lecturerToUpdate.Id} do not exist");
                return BadRequest($"User is not existed");
            }
        }
    }
}
