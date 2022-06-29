using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using AutoMapper;
using CapstoneOnGoing.Logger;
using CapstoneOnGoing.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Dtos;
using Models.Models;
using Models.Request;
using Models.Response;

namespace CapstoneOnGoing.Controllers
{
	[Route("api/v1/teams")]
	[ApiController]
	public class TeamController : ControllerBase
	{
		private readonly ILoggerManager _logger;
		private readonly ITeamService _teamService;
		private readonly IReportService _reportService;
        private readonly IUserService _userService;
		private readonly IMapper _mapper;

        public TeamController(ILoggerManager logger, ITeamService teamService, IReportService reportService, IUserService userService, IMapper mapper)
        {
            _logger = logger;
            _teamService = teamService;
            _reportService = reportService;
            _userService = userService;
            _mapper = mapper;
        }

        [Authorize(Roles = "STUDENT")]
		[HttpPost]
		[ProducesResponseType(typeof(CreatedTeamResponse), StatusCodes.Status201Created)]
		[ProducesResponseType(typeof(GenericResponse), StatusCodes.Status400BadRequest)]
		public IActionResult CreateTeam(CreateTeamRequest createTeamRequest)
		{
			string userEmail = HttpContext.User.FindFirstValue(ClaimTypes.Email);
			bool isSuccessful = _teamService.CreateTeam(createTeamRequest,userEmail, out CreatedTeamResponse createdTeamResponse);
			if (isSuccessful)
			{
				return CreatedAtAction(nameof(CreateTeam),createdTeamResponse);
			}
			else
			{
				_logger.LogWarn($"Controller: {nameof(TeamController)},Method: {nameof(CreateTeam)}: Fail to create team");
				return BadRequest(new GenericResponse()
				{
					HttpStatus = (int)HttpStatusCode.BadRequest,
					Message = "Create team failed",
					TimeStamp = DateTime.Now,
				});
			}
		}

		[Authorize(Roles = "STUDENT")]
		[HttpDelete("{id}")]
		[ProducesResponseType(typeof(GenericResponse), StatusCodes.Status400BadRequest)]
		public IActionResult DeleteTeam(Guid id)
		{
			string userEmail = HttpContext.User.FindFirstValue(ClaimTypes.Email);
			bool isSuccessful = _teamService.DeleteTeam(id, userEmail);
			if (isSuccessful)
			{
				return NoContent();
			}
			else
			{
				_logger.LogWarn($"Controller: {nameof(TeamController)},Method: {nameof(DeleteTeam)}: Fail to delete team");
				return BadRequest(new GenericResponse()
				{
					HttpStatus = (int)HttpStatusCode.BadRequest,
					Message = "Delete team failed",
					TimeStamp = DateTime.Now,
				});
			}
		}

		// [Authorize(Roles = "ADMIN,LECTURER,STUDENT,COMPANY")]
		[HttpGet]
		[ProducesResponseType(typeof(GetTeamResponse),StatusCodes.Status200OK)]
		public IActionResult GetAllTeams([FromQuery] string teamName , [FromQuery] int page, [FromQuery] int limit){
			IEnumerable<GetTeamResponse> teamsResponse = _teamService.GetAllTeams(teamName,page,limit);
			return Ok(teamsResponse);
		}

		[Authorize(Roles = "STUDENT")]
		[HttpPatch]
		[ProducesResponseType(typeof(GetTeamDetailResponse),StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(GenericResponse),StatusCodes.Status400BadRequest)]
		public IActionResult JoinTeam(JoinTeamRequest joinTeamRequest)
		{
			if (joinTeamRequest != null && "add".Equals(joinTeamRequest.Op) && "student".Equals(joinTeamRequest.Path.Replace("/",string.Empty)))
			{
				string studentEmail = HttpContext.User.FindFirstValue(ClaimTypes.Email);
				bool isSuccessful = _teamService.JoinTeam(studentEmail,joinTeamRequest.JoinCode, out GetTeamDetailResponse getTeamDetailResponse);
				if (isSuccessful)
				{
					
					return Ok(getTeamDetailResponse);
				}
				else
				{
					_logger.LogWarn($"Controller {nameof(TeamController)}, Method {nameof(JoinTeam)} : {studentEmail} join team {joinTeamRequest.JoinCode} failed");
					return BadRequest(new GenericResponse()
					{
						HttpStatus = StatusCodes.Status400BadRequest,
 						Message = "Join team Failed",
						TimeStamp = DateTime.Now
					});
				}
			}
			else
			{
				return BadRequest();
			}
		}

		// [Authorize(Roles = "ADMIN,LECTURER,STUDENT")]
		[HttpGet("{id}")]
		[ProducesResponseType(typeof(GetTeamDetailResponse), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(GenericResponse), StatusCodes.Status400BadRequest)]
		public IActionResult GetTeamDetail(Guid id)
		{
			GetTeamDetailResponse teamDetailResponse = _teamService.GetTeamDetail(id);
			if (teamDetailResponse != null)
			{
				return Ok(teamDetailResponse);
			}
			else
			{
				_logger.LogWarn($"Controller: {nameof(TeamController)}, Method: {nameof(GetTeamDetail)}: Team with {id} is not existed");
				return NotFound(new GenericResponse()
				{
					HttpStatus = StatusCodes.Status404NotFound,
					Message = "Team is not exist",
					TimeStamp = DateTime.Now
				});
			}
		}

        [Authorize(Roles = "ADMIN")]
        [HttpPatch("mentor")]
        [ProducesResponseType(typeof(GetTeamDetailResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorDetails), StatusCodes.Status400BadRequest)]
		public IActionResult UpdateTeamMentor(UpdateMentorRequest updateMentorRequest)
        {
            Guid responseUpdatedTeamId = _teamService.UpdateTeamMentor(updateMentorRequest);
			//Show team detail after update new mentor (add new or delete)
            if (!responseUpdatedTeamId.Equals(Guid.Empty))
            {
                GetTeamDetailResponse updatedMentorTeam = _teamService.GetTeamDetail(responseUpdatedTeamId);
                return Ok(updatedMentorTeam);
            }
            else
            {
                return BadRequest(new GenericResponse()
                {
                    HttpStatus = 400,
                    Message = "Update team mentor failed!",
                    TimeStamp = DateTime.Now
                });
            }
        }

        [Authorize]
        [HttpGet("{id}/reports")]
        public IActionResult GetTeamReport(Guid id)
        {
            return Ok();
        }

        [Authorize(Roles = "STUDENT")]
        [HttpPost("{id}/reports")]
        [ProducesResponseType(typeof(GenericResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(GenericResponse), StatusCodes.Status400BadRequest)]
        public IActionResult CreateWeeklyReport(Guid id, CreateWeeklyReportRequest createWeeklyReportRequest)
        {
            bool isSuccessful;
            string userEmail = HttpContext.User.FindFirstValue(ClaimTypes.Email);
            CreateWeeklyReportDTO createWeeklyReportDto = _mapper.Map<CreateWeeklyReportDTO>(createWeeklyReportRequest);

            User userByEmail = _userService.GetUserWithRoleByEmail(userEmail);
            bool isTeamLeader = _teamService.IsTeamLeader(userByEmail.Id);
            isSuccessful = _reportService.CreateWeeklyReport(id, userEmail, createWeeklyReportDto);
            if (isSuccessful)
            {
                return CreatedAtAction(nameof(CreateWeeklyReport), new GenericResponse()
                {
                    HttpStatus = StatusCodes.Status200OK,
                    Message = (isTeamLeader && createWeeklyReportRequest.IsTeamReport) ? 
                        "Create team weekly report successfully" 
                        : 
                        "Create personal weekly report successfully",
                    TimeStamp = DateTime.Now
                });
            }
            else
            {
                _logger.LogWarn((isTeamLeader && createWeeklyReportRequest.IsTeamReport) ? 
                    $"Controller: {nameof(TeamController)}, Method: {nameof(CreateWeeklyReport)}: create team weekly report failed" 
                    : 
                    $"Controller: {nameof(TeamController)}, Method: {nameof(CreateWeeklyReport)}: create personal weekly report failed");
                return BadRequest(new GenericResponse()
                {
                    HttpStatus = StatusCodes.Status400BadRequest,
                    Message = "Create weekly report failed",
                    TimeStamp = DateTime.Now
                });
            }
        }
    }
}

