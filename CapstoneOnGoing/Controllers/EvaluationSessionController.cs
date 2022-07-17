using System;
using System.Collections.Generic;
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

namespace CapstoneOnGoing.Controllers
{
	[Route("api/v1/[controller]")]
	[ApiController]
	public class EvaluationSessionController : ControllerBase
	{
		private readonly ILoggerManager _logger;
		private readonly IEvaluationSessionService _evaluationSessionService;

		public EvaluationSessionController(ILoggerManager logger, IEvaluationSessionService evaluationSessionService)
		{
			_logger = logger;
			_evaluationSessionService = evaluationSessionService;
		}

		[Authorize(Roles = "ADMIN")]
		[HttpPut("{id}")]
		[ProducesResponseType(typeof(GenericResponse),StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(GenericResponse),StatusCodes.Status400BadRequest)]
		public IActionResult UpdateEvaluationSession(UpdateEvaluationSessionRequest updateEvaluationSessionRequest)
		{
			bool isSuccessful = _evaluationSessionService.UpdateEvaluationSessionStatus(updateEvaluationSessionRequest);
			if (!isSuccessful)
			{
				return BadRequest(new GenericResponse()
				{
					HttpStatus = StatusCodes.Status400BadRequest,
					Message = "Update Evaluation Session failed",
					TimeStamp = DateTime.Now
				});
			}
			return Ok(new GenericResponse()
			{
				HttpStatus = StatusCodes.Status200OK,
				Message = "Updated Evaluation Session successfully",
				TimeStamp = DateTime.Now
			});
		}

		[Authorize(Roles = "ADMIN")]
		[HttpGet]
		[ProducesResponseType(typeof(IEnumerable<GetEvaluationSessionResponse>),StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(GenericResponse),StatusCodes.Status400BadRequest)]
		public IActionResult GetAllEvaluationSession()
		{
			var headers = Request.Headers;
			StringValues currentSemester;
			if (!headers.Keys.Contains("currentSemester") || !headers.TryGetValue("currentSemester", out currentSemester))
			{
				_logger.LogWarn($"Controller: {nameof(TeamController)},Method: {nameof(GetAllEvaluationSession)}: Semester {currentSemester}");
				return BadRequest(new GenericResponse()
				{
					HttpStatus = StatusCodes.Status400BadRequest,
					Message = "Request does not have semester",
					TimeStamp = DateTime.Now
				});
			}
			else
			{
				GetSemesterDTO semesterDto = JsonConvert.DeserializeObject<GetSemesterDTO>(currentSemester.ToString());
				IEnumerable<GetEvaluationSessionResponse> evaluationSessionResponses =
					_evaluationSessionService.GetAllEvaluationSession(semesterDto.Id);
				return Ok(evaluationSessionResponses);
			}
		}

		[Authorize(Roles = "ADMIN")]
		[HttpGet("{id}")]
		[ProducesResponseType(typeof(GetEvaluationSessionResponse), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(GenericResponse), StatusCodes.Status400BadRequest)]
		public IActionResult GetEvaluationSessionById(Guid id)
		{
			var headers = Request.Headers;
			StringValues currentSemester;
			if (!headers.Keys.Contains("currentSemester") || !headers.TryGetValue("currentSemester", out currentSemester))
			{
				_logger.LogWarn($"Controller: {nameof(TeamController)},Method: {nameof(GetEvaluationSessionById)}: Semester {currentSemester}");
				return BadRequest(new GenericResponse()
				{
					HttpStatus = StatusCodes.Status400BadRequest,
					Message = "Request does not have semester",
					TimeStamp = DateTime.Now
				});
			}
			else
			{
				GetSemesterDTO semesterDto = JsonConvert.DeserializeObject<GetSemesterDTO>(currentSemester.ToString());
				GetEvaluationSessionResponse evaluationSessionResponses =
					_evaluationSessionService.GetEvaluationSessionById(id,semesterDto.Id);
				return Ok(evaluationSessionResponses);
			}
		}
	}
}
