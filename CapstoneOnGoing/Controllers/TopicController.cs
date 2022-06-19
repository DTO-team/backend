
﻿using System;
using System.Collections.Generic;
using System.Net;
 using AutoMapper;
 using CapstoneOnGoing.Logger;
using CapstoneOnGoing.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
 using Models.Dtos;
 using Models.Request;
using Models.Response;

namespace CapstoneOnGoing.Controllers
{
	[Route("api/v1/topics")]
	[ApiController]
	public class TopicController : ControllerBase
	{
		private readonly ILoggerManager _logger;
		private readonly ITopicService _topicService;
		private readonly IMapper _mapper;

		public TopicController(ILoggerManager logger, ITopicService topicService, IMapper mapper)
		{
			_logger = logger;
			_topicService = topicService;
			_mapper = mapper;
		}

		[Authorize(Roles = "ADMIN,STUDENT,LECTURER")]
		[HttpGet]
		public IActionResult GetAllTopics()
		{
			IEnumerable<GetTopicsDTO> topicsDtos =  _topicService.GetAllTopics();
			if (topicsDtos != null)
			{
				IEnumerable<GetTopicsResponse> getTopicsResponses = _mapper.Map<IEnumerable<GetTopicsResponse>>(topicsDtos);
				return Ok(getTopicsResponses);
			}
			else
			{
				_logger.LogWarn($"Controller: {nameof(TopicController)},Method: {nameof(GetAllTopics)}: Can not load topic for semester");
				return NotFound(new GenericResponse()
				{
					HttpStatus = StatusCodes.Status404NotFound,
					Message = "No topics found. Cause no Semester is not in current in-progress or No topic in current semester",
					TimeStamp = DateTime.Now,
				});
			}
		}

		[Authorize(Roles = "ADMIN")]
		[HttpPost("list")]
		public IActionResult ImportTopics(IEnumerable<ImportTopicsRequest> importTopicsRequest)
		{
			bool isSuccessful =  _topicService.ImportTopics(importTopicsRequest);
			if (isSuccessful)
			{
				return Ok(new GenericResponse {HttpStatus = (int)HttpStatusCode.OK,Message = "Import topics successfully", TimeStamp = DateTime.Now});
			}
			else
			{
				_logger.LogWarn($"Controller: {nameof(TopicController)}, Method: {nameof(ImportTopics)}: Import topics failed");
				return BadRequest(new GenericResponse
				{
					HttpStatus = (int) HttpStatusCode.BadRequest,
					Message = "Import topics failed",
					TimeStamp = DateTime.Now
				});
			}
		}
	}
}
