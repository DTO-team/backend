using CapstoneOnGoing.Logger;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CapstoneOnGoing.Controllers
{
	[Route("api/v1/[controller]")]
	[ApiController]
	public class EvaluationSessionController : ControllerBase
	{
		private readonly ILoggerManager _logger;

	}
}
