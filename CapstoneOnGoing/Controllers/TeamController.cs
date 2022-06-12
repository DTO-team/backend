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

		//[Authorize(Roles = "STUDENT")]
		[HttpPost]
		public IActionResult CreateTeam(CreateTeamRequest createTeamRequest)
		{
			bool isSuccessful = _teamService.CreateTeam(createTeamRequest, out CreatedTeamResponse result);
			if (isSuccessful)
			{
				return Ok(result);
			}
			else
			{
				return BadRequest(new GenericResponse()
				{
					HttpStatus = (int)HttpStatusCode.BadRequest,
					Message = "Create team failed",
					TimeStamp = DateTime.Now,
				});
			}
		}
	}
}
