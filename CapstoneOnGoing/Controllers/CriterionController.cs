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
using Models.Request;
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
        [ProducesResponseType(typeof(IEnumerable<CriteriaDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenericResponse), StatusCodes.Status400BadRequest)]
        public IActionResult GetAllCriterion()
        {
            IEnumerable<CriteriaDTO> criterias = _criterionService.GetAllCriteria();

            if (criterias.Any())
            {
                return Ok(criterias);
            }
            else
            {
                _logger.LogWarn($"Controller: {nameof(CriterionController)},Method: {nameof(GetAllCriterion)}: Get all criteria failed!");
                return Ok(criterias);
            }
        }

        [Authorize(Roles = "ADMIN,STUDENT,LECTURER")]
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(CriteriaDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenericResponse), StatusCodes.Status400BadRequest)]
        public IActionResult GetCriterionById(Guid id)
        {
            if (!id.Equals(Guid.Empty))
            {
                CriteriaDTO criteria = _criterionService.GetCriteriaById(id);

                if (criteria is not null)
                {
                    return Ok(criteria);
                }
                else
                {
                    _logger.LogWarn($"Controller: {nameof(CriterionController)},Method: {nameof(GetCriterionById)}: Get criteria by id failed!");
                    return Ok(criteria);
                }
            }
            else
            {
                return BadRequest(new GenericResponse()
                {
                    HttpStatus = 400,
                    Message = "Criteria id is required!",
                    TimeStamp = DateTime.Now
                });
            }
           
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost]
        [ProducesResponseType(typeof(CriteriaDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(GenericResponse), StatusCodes.Status400BadRequest)]
        public IActionResult CreateCriteria([FromBody] CreateCriteriaRequest createCriteriaRequest)
        {
            bool isSuccess = _criterionService.CreateNewCriteria(createCriteriaRequest);
            if (isSuccess)
            {
                CriteriaDTO criteriaResponse = _criterionService.GetCriteriaByCode(createCriteriaRequest.Code);
                return CreatedAtAction("CreateCriteria", criteriaResponse);
            }
            else
            {
                _logger.LogWarn($"Controller: {nameof(CriterionController)},Method: {nameof(GetCriterionById)}: Fail to create new criteria");
                return BadRequest(new GenericResponse()
                {
                    HttpStatus = 400,
                    Message = "Create new criteria failed!",
                    TimeStamp = DateTime.Now
                });
            }
        }

        [HttpPatch]
        [ProducesResponseType(typeof(CriteriaDTO), StatusCodes.Status200OK)]
        public IActionResult UpdateCriteria([FromQuery] Guid criteriaId,[FromBody] UpdateCriteriaRequest updateCriteriaRequest)
        {
            bool isSuccess = _criterionService.UpdateCriteria(criteriaId, updateCriteriaRequest);
            if (isSuccess)
            {
                CriteriaDTO criteriaResponse = _criterionService.GetCriteriaById(criteriaId);
                return Ok(criteriaResponse);
            }
            else
            {
                _logger.LogWarn($"Controller: {nameof(CriterionController)},Method: {nameof(UpdateCriteria)}: Fail to update criteria");
                return BadRequest(new GenericResponse()
                {
                    HttpStatus = 400,
                    Message = "Update criteria failed!",
                    TimeStamp = DateTime.Now
                });
            }
        }
    }
}
