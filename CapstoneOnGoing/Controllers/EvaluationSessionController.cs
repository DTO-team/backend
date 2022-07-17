using System;
using CapstoneOnGoing.Logger;
using CapstoneOnGoing.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Request;
using Models.Response;

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

		//[Authorize(Roles = "ADMIN")]
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
	}
}
