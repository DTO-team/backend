using CapstoneOnGoing.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using CapstoneOnGoing.Logger;
using Microsoft.AspNetCore.Http;
using Models.Dtos;
using Models.Models;
using Models.Response;
using Repository.Interfaces;

namespace CapstoneOnGoing.Controllers
{
    [Route("api/v1/applications")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly IApplicationService _applicationService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerManager _logger;

        public ApplicationController(IApplicationService applicationService, IUnitOfWork unitOfWork, ILoggerManager logger)
        {
            _applicationService = applicationService;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetApplicationResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenericResponse), StatusCodes.Status400BadRequest)]
        public IActionResult GetApplicationById(Guid id)
        {
            GetApplicationResponse response = new GetApplicationResponse();
            try
            {
                GetApplicationDTO applicationDto = _applicationService.GetApplicationById(id);
                if (applicationDto != null)
                {
                    //Team's information
                    ResponseApplyTeamFields applicationFields = new ResponseApplyTeamFields();
                    applicationFields.LeaderName = _unitOfWork.User.GetById(applicationDto.TeamInformation.TeamLeaderId).FullName;
                    applicationFields.TeamName = applicationDto.TeamInformation.TeamName;
                    applicationFields.TeamSemesterSeason = _unitOfWork.Semester.GetById(applicationDto.TeamInformation.TeamSemesterId).Season;
                    response.ApplyTeam = applicationFields;

                    //Team's topic information
                    ResponseTopicFields topicFields = new ResponseTopicFields();
                    Topic teamTopic = _unitOfWork.Topic.GetById(applicationDto.Topic.TopicId);
                    topicFields.TopicName = teamTopic.Name;

                    string topicDescription = applicationDto.Topic.Description;
                    if (!string.IsNullOrEmpty(topicDescription))
                    {
                        topicFields.Description = teamTopic.Description;
                    }
                    else
                    {
                        topicFields.Description = "";
                    }
                    response.Topic = topicFields;


                    //Team's application status
                    int teamStatus = applicationDto.Status;

                    if (teamStatus.Equals(1))
                    {
                        response.Status = "PENDING";
                    }
                    else if (teamStatus.Equals(2))
                    {
                        response.Status = "APPROVED";
                    }
                    else
                    {
                        response.Status = "REJECTED";
                    }
                }
            }
            catch ( Exception e)
            {
                _logger.LogWarn($"Controller: {nameof(ApplicationController)},Method: {nameof(GetApplicationById)}, The application with {id} is not existed");
                GenericResponse errorResponse = new GenericResponse() { HttpStatus = 400, Message = "Application is not existed", TimeStamp = DateTime.Now };
                return BadRequest(errorResponse);
            }
            return Ok(response);
        }
    }
}
