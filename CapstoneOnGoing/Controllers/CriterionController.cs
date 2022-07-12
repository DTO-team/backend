using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CapstoneOnGoing.Logger;
using CapstoneOnGoing.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Models.Dtos;
using Models.Response;

namespace CapstoneOnGoing.Controllers
{
    [Route("api/v1/criterions")]
    [ApiController]
    public class CriterionController : ControllerBase
    {
        private readonly ICriterionService _criterionService;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public CriterionController(ICriterionService criterionService, ILoggerManager logger, IMapper mapper)
        {
            _criterionService = criterionService;
            _logger = logger;
            _mapper = mapper;
        }



        [Authorize(Roles = "ADMIN,STUDENT,LECTURER")]
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CriterionDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenericResponse), StatusCodes.Status400BadRequest)]
        public IActionResult GetAllCriterion()
        {
            IEnumerable<CriterionDTO> criterions = _criterionService.GetAllCriterion();

            if (criterions.Any())
            {
                return Ok(criterions);
            }
            else
            {
                _logger.LogWarn($"Controller: {nameof(CriterionController)},Method: {nameof(GetAllCriterion)}: Fail to get all criterion");
                return Ok(criterions);
            }
        }

        [Authorize(Roles = "ADMIN,STUDENT,LECTURER")]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CriterionDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenericResponse), StatusCodes.Status400BadRequest)]
        public IActionResult GetCriterionById(Guid id)
        {
            CriterionDTO criterion = _criterionService.GetCriterionById(id);

            if (criterion is not null)
            {
                return Ok(criterion);
            }
            else
            {
                _logger.LogWarn($"Controller: {nameof(CriterionController)},Method: {nameof(GetCriterionById)}: Fail to get criterion by id");
                return Ok(criterion);
            }
        }
    }
}
