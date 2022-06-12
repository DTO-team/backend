using System;
using System.Net;
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
		public IActionResult CreateTeam(CreateTeamRequest createTeamRequest)
		{
			bool isSuccessful = _teamService.CreateTeam(createTeamRequest, out CreatedTeamResponse createdTeamResponse);
			if (isSuccessful)
			{
				return Ok(createdTeamResponse);
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
		[HttpDelete]
		public IActionResult DeleteTeam(DeleteTeamRequest deleteTeamRequest)
		{
			bool isSuccessful = _teamService.DeleteTeam(deleteTeamRequest);
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
	}
}
