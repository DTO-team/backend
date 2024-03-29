﻿using System;
using System.Collections.Generic;
using System.Linq;
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

		[Authorize(Roles = "ADMIN,STUDENT,LECTURER")]
		[HttpGet]
		[ProducesResponseType(typeof(IEnumerable<GetEvaluationSessionResponse>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(GenericResponse), StatusCodes.Status400BadRequest)]
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

        //evaluationSessionId
        [HttpPost("{id}/evaluationreports")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(GenericResponse), StatusCodes.Status400BadRequest)]
		public IActionResult CreateNewEvaluationReport(Guid id,
            [FromBody] CreateNewEvaluationReportRequest newEvaluationReportRequest)
        {
            bool isSuccess = _evaluationSessionService.CreateNewEvaluationSessionReport(id, newEvaluationReportRequest);
            if (isSuccess)
            {
                return CreatedAtAction("CreateNewEvaluationReport", "Create successfully!");
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

        [Authorize(Roles = "ADMIN,LECTURER,STUDENT")]
		//evaluationSessionId
        [HttpPut("evaluationreports/{evaluationReportId}")]
        public IActionResult UpdateEvaluationReport(Guid evaluationReportId,
            UpdateEvaluationReportDetailRequest newEvaluationReportDetailRequest)
        {
            bool isSuccess = _evaluationSessionService.UpdateEvaluationSessionReport(evaluationReportId, newEvaluationReportDetailRequest);
            if (isSuccess)
            {
                return Ok("Update successfully!");
            }
            else
            {
                _logger.LogWarn($"Controller: {nameof(EvaluationSessionController)},Method: {nameof(UpdateEvaluationReport)}: Update evaluation report detail failed!");
                return BadRequest(new GenericResponse()
                {
                    HttpStatus = StatusCodes.Status400BadRequest,
                    Message = "Update evaluation report detail failed!",
                    TimeStamp = DateTime.Now
                });
            }
		}

        [Authorize(Roles = "ADMIN,LECTURER,STUDENT,COMPANY")]
        [HttpGet("evaluationreports/{evaluationReportId}")]
        [ProducesResponseType(typeof(GetAllEvaluationReportResponse), StatusCodes.Status200OK)]
        public IActionResult GetAllEvaluationReportDetailByEvaluationReportId(Guid evaluationReportId)
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

            GetSemesterDTO semesterDto = JsonConvert.DeserializeObject<GetSemesterDTO>(CurrentSemester.ToString());
            GetAllEvaluationReportResponse reportsResponse =
                _evaluationSessionService.GetAllEvaluationReportById(evaluationReportId, semesterDto);
            if (reportsResponse is not null)
            {
                return Ok(reportsResponse);
            }
            else
            {
				_logger.LogWarn($"Controller: {nameof(EvaluationSessionController)},Method: {nameof(GetAllEvaluationReportDetailByEvaluationReportId)}: Get all evaluation report detail failed!");
                return Ok(reportsResponse);
            }
		}

        [Authorize(Roles = "ADMIN,LECTURER,STUDENT,COMPANY")]
        [HttpGet("evaluationreports/{evaluationReportId}/evaluationreportdetail")]
        [ProducesResponseType(typeof(GetEvaluationReportDetailResponse), StatusCodes.Status200OK)]
        public IActionResult GetEvaluationReportDetailByEvaluationReportIdAndEvaluationReportDetailId(
            Guid evaluationReportId, [FromQuery] Guid evaluationReportDetailId)
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

            GetSemesterDTO semesterDto = JsonConvert.DeserializeObject<GetSemesterDTO>(CurrentSemester.ToString());
            GetEvaluationReportDetailResponse reportResponse =
                _evaluationSessionService.GetEvaluationReportDetailById(evaluationReportId, evaluationReportDetailId,semesterDto);
            if (reportResponse is not null)
            {
                return Ok(reportResponse);
            }
            else
            {
                _logger.LogWarn($"Controller: {nameof(EvaluationSessionController)},Method: {nameof(GetAllEvaluationReportDetailByEvaluationReportId)}: Get all evaluation report detail failed!");
                return Ok(reportResponse);
            }
        }

        [Authorize(Roles = "ADMIN,LECTURER,STUDENT,COMPANY")]
        [HttpGet("evaluationreports/reviews/{projectId}")]
        [ProducesResponseType(typeof(GetCouncilReviewOnProjectResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenericResponse), StatusCodes.Status400BadRequest)]
        public IActionResult GetAllReviewOnProjectById(Guid projectId)
        {
            IEnumerable<GetCouncilReviewOnProjectResponse> reviewsOnProject = _evaluationSessionService.GetAllReviewsOnProject(projectId);
            if (reviewsOnProject.Any())
            {
                return Ok(reviewsOnProject);
            }
            else
            {
                _logger.LogWarn($"Controller: {nameof(EvaluationSessionController)},Method: {nameof(GetReviewOnProjectById)}: Get Review On Project By Id failed!");
                return Ok(reviewsOnProject);
            }
        }

        [Authorize(Roles = "ADMIN,LECTURER,STUDENT,COMPANY")]
        [HttpGet("evaluationreports/reviews/{reviewId}/reviewDetail")]
        [ProducesResponseType(typeof(GetCouncilReviewOnProjectResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenericResponse), StatusCodes.Status400BadRequest)]
        public IActionResult GetReviewOnProjectById(Guid reviewId)
        {
            GetCouncilReviewOnProjectResponse reviewOnProject = _evaluationSessionService.GetReviewOnProjectById(reviewId);
            if (reviewOnProject is not null)
            {
                return Ok(reviewOnProject);
            }
            else
            {
                _logger.LogWarn($"Controller: {nameof(EvaluationSessionController)},Method: {nameof(GetReviewOnProjectById)}: Get Review On Project By Id failed!");
                return BadRequest(new GenericResponse()
                {
                    HttpStatus = StatusCodes.Status400BadRequest,
                    Message = "Get Review On Project By Id failed!",
                    TimeStamp = DateTime.Now
                });
            }
        }
    }
}
