using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CapstoneOnGoing.Enums;
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
                _logger.LogWarn($"Controller: {nameof(CriterionController)},Method: {nameof(GetCriterionById)}: Create new criteria failed");
                return BadRequest(new GenericResponse()
                {
                    HttpStatus = 400,
                    Message = "Create new criteria failed!",
                    TimeStamp = DateTime.Now
                });
            }
        }

        [Authorize (Roles = "ADMIN")]
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
                _logger.LogWarn($"Controller: {nameof(CriterionController)},Method: {nameof(UpdateCriteria)}: Update criteria failed!");
                return BadRequest(new GenericResponse()
                {
                    HttpStatus = 400,
                    Message = "Update criteria failed!",
                    TimeStamp = DateTime.Now
                });
            }
        }

        [Authorize(Roles = "ADMIN")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(GenericResponse), StatusCodes.Status400BadRequest)]
        public IActionResult DeleteCriteria([FromQuery] Guid criteriaId)
        {
            bool isSuccess = _criterionService.DeleteCriteria(criteriaId);

            if (isSuccess)
            {
                return NoContent();
            }
            else
            {
                _logger.LogWarn($"Controller: {nameof(CriterionController)},Method: {nameof(DeleteCriteria)}: Delete criteria failed!");
                return BadRequest(new GenericResponse()
                {
                    HttpStatus = 400,
                    Message = "Delete criteria failed!",
                    TimeStamp = DateTime.Now
                });
            }
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost("{id}/grade")]
        [ProducesResponseType(typeof(CriteriaDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(GenericResponse), StatusCodes.Status400BadRequest)]
        public IActionResult CreateNewCriteriaGrade(Guid id, CreateNewCriteriaGradeRequest newCriteriaGradeRequest)
        {
            string gradeLevel = newCriteriaGradeRequest.Level.ToUpper();

            if (!(gradeLevel.Equals(GradeLevels.ACCEPTABLE.ToString())
                  || gradeLevel.Equals(GradeLevels.EXCELLENT.ToString())
                  || gradeLevel.Equals(GradeLevels.GOOD.ToString())
                  || gradeLevel.Equals(GradeLevels.FAIL.ToString())))
            {
                throw new BadHttpRequestException(
                    $"Grade level on grade only have 4 type: ACCEPTABLE, EXCELLENT, GOOD, FAIL");
            }

            if (newCriteriaGradeRequest.MaxPoint.Equals(0) || newCriteriaGradeRequest.MinPoint.Equals(0))
            {
                throw new BadHttpRequestException("Max point or Min point is cannot be 0!");
            }

            if (newCriteriaGradeRequest.MaxPoint < newCriteriaGradeRequest.MinPoint)
            {
                int minPoint = newCriteriaGradeRequest.MaxPoint;
                newCriteriaGradeRequest.MaxPoint = newCriteriaGradeRequest.MinPoint;
                newCriteriaGradeRequest.MinPoint = minPoint;
            }

            bool isSuccess = _criterionService.CreateNewCriteriaGrade(id,newCriteriaGradeRequest);
            if (isSuccess)
            {
                CriteriaDTO criteriaResponse = _criterionService.GetCriteriaById(id);
                return CreatedAtAction("CreateNewCriteriaGrade", criteriaResponse);
            }
            else
            {
                _logger.LogWarn($"Controller: {nameof(CriterionController)},Method: {nameof(CreateNewCriteriaGrade)}: Create new criteria grade failed!");
                return BadRequest(new GenericResponse()
                {
                    HttpStatus = 400,
                    Message = "Create new criteria grade failed!",
                    TimeStamp = DateTime.Now
                });
            }
        }

        [Authorize(Roles = "ADMIN")]
        [HttpPost("{id}/question")]
        [ProducesResponseType(typeof(CriteriaDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(GenericResponse), StatusCodes.Status400BadRequest)]
        public IActionResult CreateNewCriteriaQuestion(Guid id,
            CreateNewCriteriaQuestionRequest newCriteriaQuestionRequest)
        {
            bool isSuccess = _criterionService.CreateNewQuestionGrade(id, newCriteriaQuestionRequest);
            if (isSuccess)
            {
                CriteriaDTO criteriaResponse = _criterionService.GetCriteriaById(id);
                return CreatedAtAction("CreateNewCriteriaQuestion", criteriaResponse);
            }
            else
            {
                _logger.LogWarn($"Controller: {nameof(CriterionController)},Method: {nameof(CreateNewCriteriaQuestion)}: Create new criteria question failed!");
                return BadRequest(new GenericResponse()
                {
                    HttpStatus = 400,
                    Message = "Create new criteria question failed!",
                    TimeStamp = DateTime.Now
                });
            }
        }

    }
}
