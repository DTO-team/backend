using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using CapstoneOnGoing.Logger;
using CapstoneOnGoing.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models;
using Models.Models;
using Models.Request;
using Models.Response;
using Repository.Interfaces;

namespace CapstoneOnGoing.Controllers
{
	[Route("api/v1/teams")]
	[ApiController]
	public class TeamController : ControllerBase
	{
		private readonly ILoggerManager _logger;
		private readonly ITeamService _teamService;
		public TeamController(ILoggerManager logger, ITeamService teamService)
		{
			_logger = logger;
			_teamService = teamService;
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

		[Authorize(Roles = "ADMIN,LECTURER,STUDENT,COMPANY")]
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

    }
}
