using CapstoneOnGoing.Logger;
using CapstoneOnGoing.Services.Interfaces;
using CapstoneOnGoing.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Models;
using System.IdentityModel.Tokens.Jwt;

namespace CapstoneOnGoing.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{

		private readonly ILoggerManager _logger;
		private readonly IUserService _userService;
		public AuthController(ILoggerManager logger, IUserService userService)
		{
			_logger = logger;
			_userService = userService;
		}

		[HttpPost("login")]
		public IActionResult Login([FromBody]string idToken){

			JwtSecurityToken validatedJwtToken = JwtUtil.ValidateToken(idToken);
			string email = JwtUtil.GetEmailFromJwtToken(validatedJwtToken);
			User user = _userService.GetUserByUserEmail(email);
			string jwtToken = JwtUtil.GenerateJwtToken(user.Email, user.Role.Name);
			return Ok(jwtToken);
		}
	}
}
