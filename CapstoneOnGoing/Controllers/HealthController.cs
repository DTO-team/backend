using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repository.Interfaces;
using System;
using System.Net;

namespace CapstoneOnGoing.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {

        public HealthController()
        {

        }

        [HttpGet]
        public IActionResult HealthCheck()
        {
            return Ok(new {Status = 1});
        }
    }
}
