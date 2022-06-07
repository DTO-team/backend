using System;
using System.Collections.Generic;
using System.Net;
using AutoMapper;
using CapstoneOnGoing.Logger;
using CapstoneOnGoing.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Request;
using Models.Response;

namespace CapstoneOnGoing.Controllers
{
	[Route("api/ongoing-students")]
	[ApiController]
	public class OnGoingStudentsController : ControllerBase
	{
		private readonly IMapper _mapper;
		private readonly IUserService _userService;
		private readonly ILoggerManager _logger;

		public OnGoingStudentsController(IMapper mapper, IUserService userService, ILoggerManager logger)
		{
			_mapper = mapper;
			_userService = userService;
			_logger = logger;
		}

		[Authorize(Roles = "ADMIN")]
		[HttpPost]
		public IActionResult ImportInProgressStudents([FromBody] IEnumerable<InProgressStudentsRequest> inProgressStudentsRequest)
		{
			bool isImportSuccessfully = _userService.ImportInProgressStudents(inProgressStudentsRequest);
			if (isImportSuccessfully)
			{
				return Ok(new GenericResponse 
				{
					HttpStatus = (int)HttpStatusCode.OK,
					Message = "Import in-progress students successfully",
					TimeStamp = DateTime.Now,
				});
			}
			else
			{
				_logger.LogWarn(
					$"Controller: {nameof(OnGoingStudentsController)}, Method: {nameof(ImportInProgressStudents)}, Import in-progress students list failed");
				return BadRequest(new GenericResponse
				{
					HttpStatus = (int)HttpStatusCode.BadRequest,
					Message = "Import in-progress students failed",
					TimeStamp = DateTime.Now,
				});
			}
		}

	}
}
