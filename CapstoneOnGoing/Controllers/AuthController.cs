using CapstoneOnGoing.Logger;
using CapstoneOnGoing.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace CapstoneOnGoing.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{

		private readonly ILoggerManager _logger;
		public AuthController(ILoggerManager logger)
		{
			_logger = logger;
		}

		[HttpPost("login")]
		public IActionResult Login([FromBody]string idToken){

			JwtSecurityToken jwtToken = JwtUtil.ValidateToken(idToken);
			string email = JwtUtil.GetEmailFromJwtToken(jwtToken);
			
			return Ok();
		}
	}
}
