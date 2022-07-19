using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CapstoneOnGoing.Logger;
using CapstoneOnGoing.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Models.Dtos;
using Models.Request;
using Models.Response;
using Newtonsoft.Json;
using Repository.Interfaces;

namespace CapstoneOnGoing.Controllers
{
	[Route("api/v1/councils")]
	[ApiController]
	public class CouncilController : ControllerBase
	{
		private readonly ILoggerManager _logger;
		private readonly IMapper _mapper;
		private readonly ICouncilService _councilService;
        private readonly IProjectService _projectService;

		public CouncilController( ILoggerManager logger, IMapper mapper, ICouncilService countService)
		{
			_logger = logger;
			_mapper = mapper;
			_councilService = countService;
		}

		[HttpGet("{councilId}/projects")]
		public IActionResult GetAllCouncilProjects(Guid councilId)
		{
			var headers = Request.Headers;
			StringValues CurrentSemester;
			if (!headers.Keys.Contains("currentsemester") || !headers.TryGetValue("currentsemester", out CurrentSemester))
			{
				_logger.LogWarn($"Controller: {nameof(TeamController)},Method: {nameof(GetAllCouncilProjects)}: Semester {CurrentSemester}");
				return BadRequest(new GenericResponse()
				{
					HttpStatus = StatusCodes.Status400BadRequest,
					Message = "Request does not have semester",
					TimeStamp = DateTime.Now
				});
			}

			GetSemesterDTO semesterDto = JsonConvert.DeserializeObject<GetSemesterDTO>(CurrentSemester.ToString());
			IEnumerable<GetProjectDetailDTO> allCouncilProject = _projectService.GetAllCouncilProject(councilId, semesterDto);
			List<GetProjectDetailResponse> allCouncilProjectResponse = new List<GetProjectDetailResponse>();
			if (allCouncilProject.Any())
			{
				allCouncilProjectResponse.ForEach(councilProject =>
				{

				});
				return Ok();
			}
			else
			{
				return Ok();
			}
		}

		[Authorize(Roles = "ADMIN")]
		[HttpGet]
		[ProducesResponseType(typeof(IEnumerable<GetCouncilResponse>),StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(GenericResponse),StatusCodes.Status400BadRequest)]
		public IActionResult GetAllCouncils()
		{
			var headers = Request.Headers;
			StringValues CurrentSemester;
			if (!headers.Keys.Contains("currentsemester") || !headers.TryGetValue("currentsemester", out CurrentSemester))
			{
				_logger.LogWarn($"Controller: {nameof(TeamController)},Method: {nameof(GetAllCouncils)}: Semester {CurrentSemester}");
				return BadRequest(new GenericResponse()
				{
					HttpStatus = StatusCodes.Status400BadRequest,
					Message = "Request does not have semester",
					TimeStamp = DateTime.Now
				});
			}

			GetSemesterDTO semesterDto = JsonConvert.DeserializeObject<GetSemesterDTO>(CurrentSemester.ToString());
			IEnumerable<GetCouncilResponse> councilResponses = _councilService.GetAllCouncils(semesterDto);
			return Ok(councilResponses);
		}

		[Authorize(Roles = "ADMIN")]
		[HttpGet("{id}")]
		[ProducesResponseType(typeof(GetCouncilResponse), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(GenericResponse), StatusCodes.Status400BadRequest)]
		public IActionResult GetCouncilById(Guid id)
		{
			var headers = Request.Headers;
			StringValues CurrentSemester;
			if (!headers.Keys.Contains("currentsemester") || !headers.TryGetValue("currentsemester", out CurrentSemester))
			{
				_logger.LogWarn($"Controller: {nameof(TeamController)},Method: {nameof(GetAllCouncils)}: Semester {CurrentSemester}");
				return BadRequest(new GenericResponse()
				{
					HttpStatus = StatusCodes.Status400BadRequest,
					Message = "Request does not have semester",
					TimeStamp = DateTime.Now
				});
			}

			GetSemesterDTO semesterDto = JsonConvert.DeserializeObject<GetSemesterDTO>(CurrentSemester.ToString());
			GetCouncilResponse councilResponse = _councilService.GetCouncilById(id, semesterDto);
			return Ok(councilResponse);
		}
	}
}
