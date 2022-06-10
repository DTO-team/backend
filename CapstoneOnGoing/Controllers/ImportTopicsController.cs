using System.Collections.Generic;
using CapstoneOnGoing.Logger;
using CapstoneOnGoing.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Request;

namespace CapstoneOnGoing.Controllers
{
	[Route("api/import-topics")]
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

		[HttpPost]
		public IActionResult ImportTopics(IEnumerable<ImportTopicsRequest> importTopicsRequest)
		{
			_topicService.ImportTopics(importTopicsRequest);
			return Ok();
		}
	}
}
