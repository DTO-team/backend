﻿using System;
using CapstoneOnGoing.Logger;
using CapstoneOnGoing.Services.Interfaces;
using CapstoneOnGoing.Utils;
using Microsoft.AspNetCore.Mvc;
using Models.Dtos;
using Models.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using AutoMapper;
using CapstoneOnGoing.Enums;
using Microsoft.IdentityModel.Tokens;
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
		public IActionResult Login([FromBody] CognitoIdToken cognitoIdToken)
		{
			try
			{
				JwtSecurityToken validatedJwtToken = JwtUtil.ValidateToken(cognitoIdToken.IdToken);
				(string email, string name) result = JwtUtil.GetEmailFromJwtToken(validatedJwtToken);
				User user = _userService.GetUserWithRoleByEmail(result.email);
				//if user does not exist, create a new account for user
				if (user == null)
				{
					user = _userService.CreateUserByEmailAndName(result.email, result.name);
					//Create User Failed
					if (user == null)
					{
						return BadRequest(new GenericResponse
						{
							HttpStatus = (int) HttpStatusCode.BadRequest, Message = "Login Failed",
							TimeStamp = DateTime.Now
						});
					}
				}

				string accessToken = JwtUtil.GenerateJwtToken(user.Email, user.Role.Name);
                LoginUserResponse loginUserResponse = null;
                switch (user.RoleId)
                {
					case 1:
                        loginUserResponse = _mapper.Map<LoginUserAdminResponse>(user);
                        loginUserResponse.Status = new UserStatusResponse()
                        {
                            StatusId = user.StatusId,
                            StatusName = user.StatusId.Equals(1)
                                ? UserStatus.Activated.ToString().ToUpper()
                                : UserStatus.Inactivated.ToString().ToUpper()
                        };
						loginUserResponse.AccessToken = accessToken;
						break;
					case 2:
						loginUserResponse =
							_mapper.Map<LoginUserLecturerResponse>(user);
                        loginUserResponse.Status = new UserStatusResponse()
                        {
                            StatusId = user.StatusId,
                            StatusName = user.StatusId.Equals(1)
                                ? UserStatus.Activated.ToString().ToUpper()
                                : UserStatus.Inactivated.ToString().ToUpper()
                        };
						loginUserResponse.AccessToken = accessToken;
						break;
					case 3:
						loginUserResponse = _mapper.Map<LoginUserStudentResponse>(user);
                        loginUserResponse.Status = new UserStatusResponse()
                        {
                            StatusId = user.StatusId,
                            StatusName = user.StatusId.Equals(1)
                                ? UserStatus.Activated.ToString().ToUpper()
                                : UserStatus.Inactivated.ToString().ToUpper()
                        };
						loginUserResponse.AccessToken = accessToken;
                        break;
					case 4:
						loginUserResponse = _mapper.Map<LoginUserCompanyResponse>(user);
                        loginUserResponse.Status = new UserStatusResponse()
                        {
                            StatusId = user.StatusId,
                            StatusName = user.StatusId.Equals(1)
                                ? UserStatus.Activated.ToString().ToUpper()
                                : UserStatus.Inactivated.ToString().ToUpper()
                        };
						loginUserResponse.AccessToken = accessToken;
						break;
				}

				return Ok(loginUserResponse);
			}
			catch (ArgumentException e)
			{
				_logger.LogWarn($"Controller: {nameof(AuthController)}, Method: {nameof(Login)}: {e.Message}");
				return BadRequest(new GenericResponse
				{
					HttpStatus = (int) HttpStatusCode.BadRequest, Message = "Login Failed: Jwt have wrong format",
					TimeStamp = DateTime.Now
				});
			}
			catch (SecurityTokenExpiredException e)
			{
				_logger.LogWarn($"Controller: {nameof(AuthController)}, Method: {nameof(Login)}: {e.Message}");
				return BadRequest(new GenericResponse
				{
					HttpStatus = (int)HttpStatusCode.BadRequest,
					Message = "Login Failed: jwt token expired",
					TimeStamp = DateTime.Now
				});
			}
			catch (SecurityTokenValidationException e)
			{
				_logger.LogWarn($"Controller: {nameof(AuthController)}, Method: {nameof(Login)}: {e.Message}");
				return BadRequest(new GenericResponse
				{
					HttpStatus = (int)HttpStatusCode.BadRequest,
					Message = $"Login Failed: {e.Message}",
					TimeStamp = DateTime.Now
				});
			}
			catch (Exception e)
			{
				_logger.LogWarn($"Controller: {nameof(AuthController)}, Method: {nameof(Login)}: {e.Message}");
				return BadRequest(new GenericResponse
				{
					HttpStatus = (int)HttpStatusCode.BadRequest,
					Message = $"Login Failed",
					TimeStamp = DateTime.Now
				});
			}
		}
	}
}
