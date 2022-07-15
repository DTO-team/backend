using System;
using AutoMapper;
using CapstoneOnGoing.Logger;
using CapstoneOnGoing.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Request;
using Models.Response;
using Repository.Interfaces;

namespace CapstoneOnGoing.Controllers
{
	[Route("api/v1/[controller]")]
	[ApiController]
	public class CouncilController : ControllerBase
	{
		private readonly ILoggerManager _logger;
		private readonly IMapper _mapper;
		private readonly ICouncilService _councilService;

		public CouncilController( ILoggerManager logger, IMapper mapper, ICouncilService countService)
		{
			_logger = logger;
			_mapper = mapper;
			_councilService = countService;
		}

		[Authorize(Roles = "ADMIN")]
		[HttpPost]
		public IActionResult CreateCouncil([FromBody] CreateCouncilRequest createCouncilRequest)
		{
			bool isSuccessful = _councilService.CreateCouncil(createCouncilRequest);
			if (!isSuccessful)
			{
				_logger.LogWarn($"Controller: {nameof(CouncilController)}, Method: {nameof(CreateCouncil)}: Create Council failed");
				return BadRequest(new GenericResponse()
				{
					HttpStatus = StatusCodes.Status400BadRequest,
					Message = "Create Council failed",
					TimeStamp = DateTime.Now
				});
			}

			return CreatedAtAction(nameof(CreateCouncil),new GenericResponse()
			{
				HttpStatus = StatusCodes.Status201Created,
				Message = "Create council successfully",
				TimeStamp = DateTime.Now,
			});
		}
	}
}
