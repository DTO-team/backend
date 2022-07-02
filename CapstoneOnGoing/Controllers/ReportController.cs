﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using AutoMapper;
using CapstoneOnGoing.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Models.Dtos;
using Models.Models;
using Models.Request;
using Models.Response;

namespace CapstoneOnGoing.Controllers
{
    [Route("api/v1/reports")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IReportService _reportService;
        private readonly IStudentService _studentService;
        private readonly IMapper _mapper;


        public ReportController(IReportService reportService, IStudentService studentService, IMapper mapper)
        {
            _reportService = reportService;
            _studentService = studentService;
            _mapper = mapper;
        }

        //team id
        [Authorize (Roles = "STUDENT")]
        [HttpPost("{id}")]
        [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(GenericResponse), StatusCodes.Status400BadRequest)]
        public IActionResult CreatePersonalOrTeamWeeklyReport([FromBody] CreateWeeklyReportRequest createWeeklyReportRequest, Guid id)
        {
            string userEmail = HttpContext.User.FindFirstValue(ClaimTypes.Email);
            User studentUser = _studentService.GetStudentByEmail(userEmail);
            if (studentUser is not null)
            {
                IEnumerable<ReportEvidenceDTO> reportEvidenceDto = null;
                if (!createWeeklyReportRequest.ReportEvidences.Count().Equals(0))
                {
                    reportEvidenceDto = _mapper.Map<IEnumerable<ReportEvidenceDTO>>(createWeeklyReportRequest.ReportEvidences);
                }

                CreateWeeklyReportDTO createWeeklyReportDto = _mapper.Map<CreateWeeklyReportDTO>(createWeeklyReportRequest);
                createWeeklyReportDto.ReportEvidences = reportEvidenceDto;

                bool isCreated = _reportService.CreateWeeklyReport(id, userEmail, createWeeklyReportDto);
                if (isCreated)
                {
                    return CreatedAtAction("CreatePersonalOrTeamWeeklyReport", id);
                }
                else
                {
                    return BadRequest(new GenericResponse()
                    { HttpStatus = 400, Message = "Create weekly report failed", TimeStamp = DateTime.Now });
                }
            }
            else
            {
                return BadRequest(new GenericResponse() { HttpStatus = 400, Message = "Create weekly report only for student!", TimeStamp = DateTime.Now });
            }
        }
    }
}