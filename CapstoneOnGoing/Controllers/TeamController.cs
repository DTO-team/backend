﻿	using System;
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

		[Authorize(Roles = "ADMIN,LECTURER,STUDENT")]
		[HttpGet]
		[ProducesResponseType(typeof(GetTeamResponse),StatusCodes.Status200OK)]
		public IActionResult GetAllTeams([FromQuery] string teamName , [FromQuery] int page, [FromQuery] int limit){
			IEnumerable<GetTeamResponse> teamsResponse = _teamService.GetAllTeams(teamName,page,limit);
			return Ok(teamsResponse);
		}

		[Authorize(Roles = "STUDENT")]
		[HttpPatch("{id}")]
		[ProducesResponseType(typeof(GetTeamDetailResponse),StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(GenericResponse),StatusCodes.Status400BadRequest)]
		public IActionResult JoinTeam(Guid id,JoinTeamRequest joinTeamRequest)
		{
			if (joinTeamRequest != null && "add".Equals(joinTeamRequest.Op) && "student".Equals(joinTeamRequest.Path.Replace("/",string.Empty)))
			{
				string studentEmail = HttpContext.User.FindFirstValue(ClaimTypes.Email);
				bool isSuccessful = _teamService.JoinTeam(id, studentEmail,joinTeamRequest.JoinCode, out GetTeamDetailResponse getTeamDetailResponse);
				if (isSuccessful)
				{
					
					return Ok(getTeamDetailResponse);
				}
				else
				{
					_logger.LogWarn($"Controller {nameof(TeamController)}, Method {nameof(JoinTeam)} : {studentEmail} join team {id} failed");
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
	}
}
