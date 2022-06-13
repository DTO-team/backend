using AutoMapper;
using CapstoneOnGoing.Logger;
using CapstoneOnGoing.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;

namespace CapstoneOnGoing.Controllers
{
    [Route("api/v1/applications")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IApplicationService _applicationService;
        private readonly ILoggerManager _logger;

        public ApplicationController(IMapper mapper, IApplicationService applicationService, ILoggerManager logger)
        {
            _mapper = mapper;
            _applicationService = applicationService;
            _logger = logger;
        }

        [HttpGet("{id}")]
        public IActionResult GetApplicationById(Guid id)
        {
            return Ok();
        }
    }
}
