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
	[Route("api/v1/evaluationsessions")]
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
		public IActionResult UpdateEvaluationSession(Guid id, UpdateEvaluationSessionRequest updateEvaluationSessionRequest)
		{
			bool isSuccessful = _evaluationSessionService.UpdateEvaluationSessionStatus(id, updateEvaluationSessionRequest);
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
		public IActionResult GetAllEvaluationSession([FromQuery] Guid semesterId)
		{
			IEnumerable<GetEvaluationSessionResponse> evaluationSessionResponses =
					_evaluationSessionService.GetAllEvaluationSession(semesterId);
				return Ok(evaluationSessionResponses);
		}

		[Authorize(Roles = "ADMIN")]
		[HttpGet("{id}")]
		[ProducesResponseType(typeof(GetEvaluationSessionResponse), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(GenericResponse), StatusCodes.Status400BadRequest)]
		public IActionResult GetEvaluationSessionById(Guid id,[FromQuery]Guid semesterId)
		{
			GetEvaluationSessionResponse evaluationSessionResponses =
					_evaluationSessionService.GetEvaluationSessionById(id, semesterId);
				return Ok(evaluationSessionResponses);
			}
		}
	}
}
