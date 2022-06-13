using CapstoneOnGoing.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using Models.Response;

namespace CapstoneOnGoing.Controllers
{
    [Route("api/v1/applications")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationService _applicationService;

        public ApplicationController(IApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [HttpGet("{id}")]
        // [ProducesResponseType(typeof(GetApplicationDTO), StatusCodes.Status200OK)]
        // [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        public IActionResult GetApplicationById(Guid id)
        {
            GetApplicationResponse result = _applicationService.GetApplicationById(id);
            if (result != null)
            {
                return Ok(result);
            }

            return BadRequest("Application is not existed");
        }
    }
}
