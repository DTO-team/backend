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
using Microsoft.Extensions.Primitives;
using Models;
using Models.Dtos;
using Models.Models;
using Models.Request;
using Models.Response;
using Newtonsoft.Json;

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
			bool isSuccessful = _teamService.CreateTeam(createTeamRequest, userEmail, out CreatedTeamResponse createdTeamResponse);
			if (isSuccessful)
			{
				return CreatedAtAction(nameof(CreateTeam), createdTeamResponse);
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

		[Authorize(Roles = "ADMIN,LECTURER,STUDENT,COMPANY")]
		[HttpGet]
		[ProducesResponseType(typeof(GetTeamResponse), StatusCodes.Status200OK)]
		public IActionResult GetAllTeams([FromQuery] string teamName, [FromQuery] int page, [FromQuery] int limit)
		{
			IEnumerable<GetTeamResponse> teamsResponse = _teamService.GetAllTeams(teamName, page, limit);
			return Ok(teamsResponse);
		}

		[Authorize(Roles = "STUDENT")]
		[HttpPatch]
		[ProducesResponseType(typeof(GetTeamDetailResponse), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(GenericResponse), StatusCodes.Status400BadRequest)]
		public IActionResult JoinTeam(JoinTeamRequest joinTeamRequest)
		{
			if (joinTeamRequest != null && "add".Equals(joinTeamRequest.Op) && "student".Equals(joinTeamRequest.Path.Replace("/", string.Empty)))
			{
				string studentEmail = HttpContext.User.FindFirstValue(ClaimTypes.Email);
				bool isSuccessful = _teamService.JoinTeam(studentEmail, joinTeamRequest.JoinCode, out GetTeamDetailResponse getTeamDetailResponse);
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

		[Authorize(Roles = "ADMIN,LECTURER,STUDENT")]
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

		[Authorize(Roles = "ADMIN,STUDENT,LECTURER")]
		[HttpGet("{id}/reports")]
		public IActionResult GetTeamReport(Guid id, [FromQuery] int week)
		{
			string userEmail = HttpContext.User.FindFirstValue(ClaimTypes.Email);
			var headers = Request.Headers;
			StringValues currentsemester;
			if (!headers.Keys.Contains("currentsemester") || !headers.TryGetValue("currentsemester", out currentsemester))
			{
				_logger.LogWarn($"Controller: {nameof(TeamController)},Method: {nameof(GetTeamReport)}: Semester {currentsemester}");
				return BadRequest(new GenericResponse()
				{
					HttpStatus = StatusCodes.Status400BadRequest,
					Message = "Request does not have semester",
					TimeStamp = DateTime.Now
				});
			}
			else
			{
				GetSemesterDTO semesterDto = JsonConvert.DeserializeObject<GetSemesterDTO>(currentsemester.ToString());
				List<GetTeamWeeklyReportResponse> teamWeeklyReportResponses =
					_reportService.GetTeamWeeklyReport(id, week, semesterDto, userEmail);
				return Ok(teamWeeklyReportResponses);
			}
		}

		[Authorize(Roles = "STUDENT")]
		[HttpPost("{id}/reports")]
		[ProducesResponseType(typeof(GenericResponse), StatusCodes.Status201Created)]
		[ProducesResponseType(typeof(GenericResponse), StatusCodes.Status400BadRequest)]
		public IActionResult CreateWeeklyReport(Guid id, CreateWeeklyReportRequest createWeeklyReportRequest)
		{
			Guid? reportId;
			string userEmail = HttpContext.User.FindFirstValue(ClaimTypes.Email);
			CreateWeeklyReportDTO createWeeklyReportDto = _mapper.Map<CreateWeeklyReportDTO>(createWeeklyReportRequest);

			User userByEmail = _userService.GetUserWithRoleByEmail(userEmail);
			bool isTeamLeader = _teamService.IsTeamLeader(userByEmail.Id);
			reportId = _reportService.CreateWeeklyReport(id, userEmail, createWeeklyReportDto);
			if (reportId is not null)
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

		[Authorize(Roles = "ADMIN,LECTURER,STUDENT")]
		[HttpGet("{id}/reports/{reportId}")]
		[ProducesResponseType(typeof(GetWeeklyReportDetailResponse), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(GenericResponse), StatusCodes.Status404NotFound)]
		public IActionResult GetWeeklyReportDetail(Guid id, Guid reportId)
		{
			string email = HttpContext.User.FindFirstValue(ClaimTypes.Email);
			GetWeeklyReportDetailResponse reportResponse = _reportService.GetReportDetail(id, reportId, email);
			if (reportResponse == null)
			{
				_logger.LogWarn($"Controller: {nameof(TeamController)}, Method: {nameof(GetWeeklyReportDetail)}: Report {reportId} is not found");
				return NotFound(new GenericResponse()
				{
					HttpStatus = StatusCodes.Status404NotFound,
					Message = "No report is found",
					TimeStamp = DateTime.Now,
				});
			}
			return Ok(reportResponse);
		}

		[Authorize(Roles = "LECTURER")]
		[HttpPatch("{id}/reports/{reportId}")]
		[ProducesResponseType(typeof(GenericResponse), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(GenericResponse), StatusCodes.Status400BadRequest)]
		public IActionResult FeedbackReport(Guid id, Guid reportId, [FromBody] FeedbackReportRequest feedbackReportRequest)
		{
			string email = HttpContext.User.FindFirstValue(ClaimTypes.Email);
			feedbackReportRequest.Path.Remove(0);
			if (!feedbackReportRequest.Op.Equals("add") || !feedbackReportRequest.Path.Equals("feedbacks") || string.IsNullOrWhiteSpace(feedbackReportRequest.Value) || string.IsNullOrEmpty(feedbackReportRequest.Value))
			{
				_logger.LogWarn($"Controller: {nameof(TeamController)}, Method: {nameof(FeedbackReport)}: Value is invalid {feedbackReportRequest.Op} - {feedbackReportRequest.Path} - {feedbackReportRequest.Value}");
				return BadRequest(new GenericResponse()
				{
					HttpStatus = StatusCodes.Status400BadRequest,
					Message = "Invalid value",
					TimeStamp = DateTime.Now
				});
			}

			bool isSuccessful = _reportService.FeedbackReport(id, reportId, email, feedbackReportRequest);
			if (!isSuccessful)
			{
				_logger.LogWarn($"Controller: {nameof(TeamController)}, Method: {nameof(FeedbackReport)}:Lecturer {email} feedback for report {reportId} is failed");
				return BadRequest(new GenericResponse()
				{
					HttpStatus = StatusCodes.Status400BadRequest,
					Message = "Feedback for report is failed",
					TimeStamp = DateTime.Now
				});
			}
			return Ok(new GenericResponse()
			{
				HttpStatus = StatusCodes.Status200OK,
				Message = "Feedback for report successful",
				TimeStamp = DateTime.Now
			});
		}
	}
}

