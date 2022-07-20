﻿using System;
using System.Collections.Generic;
using CapstoneOnGoing.Logger;
using CapstoneOnGoing.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Models.Dtos;
using Models.Models;
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
		[ProducesResponseType(typeof(GenericResponse), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(GenericResponse), StatusCodes.Status400BadRequest)]
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
		[ProducesResponseType(typeof(IEnumerable<GetEvaluationSessionResponse>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(GenericResponse), StatusCodes.Status400BadRequest)]
		public IActionResult GetAllEvaluationSession([FromQuery] Guid semesterId)
		{
			var headers = Request.Headers;
			StringValues CurrentSemester;
			if (!headers.Keys.Contains("currentsemester") || !headers.TryGetValue("currentsemester", out CurrentSemester))
			{
				_logger.LogWarn($"Controller: {nameof(EvaluationSessionController)},Method: {nameof(GetAllEvaluationSession)}: Semester {CurrentSemester}");
				return BadRequest(new GenericResponse()
				{
					HttpStatus = StatusCodes.Status400BadRequest,
					Message = "Request does not have semester",
					TimeStamp = DateTime.Now
				});
			}
			else
			{
				GetSemesterDTO semesterDto = JsonConvert.DeserializeObject<GetSemesterDTO>(CurrentSemester.ToString());
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
            StringValues CurrentSemester;
            if (!headers.Keys.Contains("currentsemester") || !headers.TryGetValue("currentsemester", out CurrentSemester))
            {
				_logger.LogWarn($"Controller: {nameof(EvaluationSessionController)},Method: {nameof(GetEvaluationSessionById)}: Semester {CurrentSemester}");
				return BadRequest(new GenericResponse()
				{
					HttpStatus = StatusCodes.Status400BadRequest,
					Message = "Request does not have semester",
					TimeStamp = DateTime.Now
				});
			}
			else
			{
				GetSemesterDTO semesterDto = JsonConvert.DeserializeObject<GetSemesterDTO>(CurrentSemester.ToString());
				GetEvaluationSessionResponse evaluationSessionResponses =
					_evaluationSessionService.GetEvaluationSessionById(id,semesterDto.Id);
				return Ok(evaluationSessionResponses);
			}
		}

		[HttpPost("review")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(GenericResponse), StatusCodes.Status400BadRequest)]
		public IActionResult CreateNewReviewOfEvaluationSession([FromBody] CreateNewReviewRequest newReviewRequest)
        {
            bool isSuccess = _evaluationSessionService.CreateNewReviewOfCouncilEvaluationSession(newReviewRequest);
            if (isSuccess)
            {
                return CreatedAtAction("CreateNewReviewOfEvaluationSession", "Create successfully!");
            }
            else
            {
				_logger.LogWarn($"Controller: {nameof(EvaluationSessionController)},Method: {nameof(CreateNewReviewOfEvaluationSession)}: Create new review failed!");
                return BadRequest(new GenericResponse()
                {
                    HttpStatus = StatusCodes.Status400BadRequest,
                    Message = "Create new review failed!",
                    TimeStamp = DateTime.Now
                });
			}
        }

        [HttpPost("{id}/evaluationreport")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(GenericResponse), StatusCodes.Status400BadRequest)]
		public IActionResult CreateNewEvaluationReport(Guid id,
            [FromBody] CreateNewEvaluationReportRequest newEvaluationReportRequest)
        {
            bool isSuccess = _evaluationSessionService.CreateNewEvaluationSessionReport(id, newEvaluationReportRequest);
            if (isSuccess)
            {
                return CreatedAtAction("CreateNewReviewOfEvaluationSession", "Create successfully!");
            }
            else
            {
				_logger.LogWarn($"Controller: {nameof(EvaluationSessionController)},Method: {nameof(CreateNewEvaluationReport)}: Create new evaluation report failed!");
                return BadRequest(new GenericResponse()
                {
                    HttpStatus = StatusCodes.Status400BadRequest,
                    Message = "Create new evaluation report failed!",
                    TimeStamp = DateTime.Now
                });
			}
		}

	}
}
