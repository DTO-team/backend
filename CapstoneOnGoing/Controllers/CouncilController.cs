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
	}
}
