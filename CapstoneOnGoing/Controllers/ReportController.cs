using CapstoneOnGoing.Logger;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CapstoneOnGoing.Controllers
{
	[Route("api/v1/report")]
	[ApiController]
	public class ReportController : ControllerBase
	{
		private readonly ILoggerManager _logger;

		public ReportController(ILoggerManager logger)
		{
			_logger = logger;
		}

		[Authorize]
		[HttpPost]
		public IActionResult CreateWeeklyReport()
		{
			return Ok();
		}

	}
}
