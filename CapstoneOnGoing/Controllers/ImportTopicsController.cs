
﻿using System;
using System.Collections.Generic;
using System.Net;
using CapstoneOnGoing.Logger;
using CapstoneOnGoing.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Request;
using Models.Response;

namespace CapstoneOnGoing.Controllers
{
	[Route("api/v1/topics")]
	[ApiController]
	public class ImportTopicsController : ControllerBase
	{
		private readonly ILoggerManager _logger;
		private readonly ITopicService _topicService;

		public ImportTopicsController(ILoggerManager logger, ITopicService topicService)
		{
			_logger = logger;
			_topicService = topicService;
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
				_logger.LogWarn($"Controller: {nameof(ImportTopicsController)}, Method: {nameof(ImportTopics)}: Import topics failed");
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
