using CapstoneOnGoing.Logger;
using CapstoneOnGoing.Services.Interfaces;
using CapstoneOnGoing.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Dtos;
using Models.Models;
using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using Models.Response;

namespace CapstoneOnGoing.Controllers
{
	[Route("api/v1/auth")]
	[ApiController]
	public class AuthController : ControllerBase
	{

		private readonly ILoggerManager _logger;
		private readonly IUserService _userService;
		private readonly IMapper _mapper;
		public AuthController(ILoggerManager logger, IUserService userService, IMapper mapper)
		{
			_logger = logger;
			_userService = userService;
			_mapper = mapper;
		}

		[HttpPost("login")]
		public IActionResult Login([FromBody]CognitoIdToken cognitoIdToken){

			JwtSecurityToken validatedJwtToken = JwtUtil.ValidateToken(cognitoIdToken.IdToken);
			(string email,string name) result = JwtUtil.GetEmailFromJwtToken(validatedJwtToken);
			User user = _userService.GetUserWithRoleByEmail(result.email);
			if (user == null)
			{
				user = _userService.CreateUserByEmailAndName(result.email, result.name);
				if (user == null)
				{
					return BadRequest($"Login Failed");
				}
			}
			string accessToken = JwtUtil.GenerateJwtToken(user.Email, user.Role.Name);
			switch (user.RoleId)
			{
				case 2:
					LoginUserLecturerResponse loginUserLecturerResponseResponse =
						_mapper.Map<LoginUserLecturerResponse>(user);
					loginUserLecturerResponseResponse.AccessToken = accessToken;
					return Ok(loginUserLecturerResponseResponse);
				case 3:
					LoginUserStudentResponse loginUserStudentResponse = _mapper.Map<LoginUserStudentResponse>(user);
					loginUserStudentResponse.AccessToken = accessToken;
					return Ok(loginUserStudentResponse);
				case 4:
					LoginUserCompanyResponse loginUserCompanyResponse = _mapper.Map<LoginUserCompanyResponse>(user);
					loginUserCompanyResponse.AccessToken = accessToken;
					return Ok(loginUserCompanyResponse);
			}

			return BadRequest($"Login Failed");
		}
	}
}
