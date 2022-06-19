using CapstoneOnGoing.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using CapstoneOnGoing.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Models.Dtos;
using Models.Models;
using Models.Request;
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

        public ApplicationController(IApplicationService applicationService, IUnitOfWork unitOfWork)
        {
            _applicationService = applicationService;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN,STUDENT")]
        [ProducesResponseType(typeof(GetApplicationResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(GenericResponse), StatusCodes.Status400BadRequest)]
        public IActionResult CreateNewApplication(CreateNewApplicationRequest newApplicationRequest)
        {
            GetApplicationResponse response = new GetApplicationResponse();
            GetApplicationDTO applicationDto = _applicationService.CreateNewApplication(newApplicationRequest);
            if (applicationDto is not null)
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
                    response.Status = ApplicationStatus.Pending.ToString().ToUpper();
                }
                else if (teamStatus.Equals(2))
                {
                    response.Status = ApplicationStatus.Approved.ToString().ToUpper();
                }
                else
                {
                    response.Status = ApplicationStatus.Rejected.ToString().ToUpper(); ;
                }

                return CreatedAtAction("CreateNewApplication", response);
            }
            else
            {
                return BadRequest(new GenericResponse()
                { HttpStatus = 400, Message = "Create New Application failed", TimeStamp = DateTime.Now });
            }
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        [ProducesResponseType(typeof(List<GetApplicationResponse>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenericResponse), StatusCodes.Status500InternalServerError)]
        public IActionResult GetAllApplications()
        {
            List<GetApplicationResponse> applicationResponses = new List<GetApplicationResponse>();
            IEnumerable<GetApplicationDTO> applicationDtos = _applicationService.GetAllApplication();

            if (applicationDtos != null)
            {
                Array.ForEach(applicationDtos.ToArray(), applicationDto =>
                {
                    GetApplicationResponse response = new GetApplicationResponse();
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
                        response.Status = ApplicationStatus.Pending.ToString().ToUpper();
                    }
                    else if (teamStatus.Equals(2))
                    {
                        response.Status = ApplicationStatus.Approved.ToString().ToUpper();
                    }
                    else
                    {
                        response.Status = ApplicationStatus.Rejected.ToString().ToUpper(); ;
                    }
                    applicationResponses.Add(response);
                });
            }

            if (applicationResponses.Any())
            {
                return Ok(applicationResponses);
            }
            else
            {
                return Ok(applicationResponses);
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "ADMIN")]
        [ProducesResponseType(typeof(GetApplicationResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenericResponse), StatusCodes.Status400BadRequest)]
        public IActionResult GetApplicationById(Guid id)
        {
            GetApplicationResponse response = new GetApplicationResponse();
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
                    response.Status = ApplicationStatus.Pending.ToString().ToUpper();
                }
                else if (teamStatus.Equals(2))
                {
                    response.Status = ApplicationStatus.Approved.ToString().ToUpper();
                }
                else
                {
                    response.Status = ApplicationStatus.Rejected.ToString().ToUpper(); ;
                }
                return Ok(response);

            }
            else
            {
                GenericResponse errorResponse = new GenericResponse() { HttpStatus = 400, Message = "Application is not existed", TimeStamp = DateTime.Now };
                return BadRequest(errorResponse);
            }
        }

        [HttpPatch("status")]
        [Authorize(Roles = "ADMIN")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenericResponse), StatusCodes.Status400BadRequest)]
        public IActionResult UpdateApplicationStatus([FromQuery] Guid id, UpdateApplicationStatusRequest request)
        {
            bool isSuccess = _applicationService.UpdateApplicationStatusById(id, request);
            if (isSuccess)
            {
                return Ok();
            }
            else
            {
                GenericResponse errorResponse = new GenericResponse()
                { HttpStatus = 400, Message = "Update status failed!", TimeStamp = DateTime.Now };
                return BadRequest(errorResponse);
            }
        }
    }
}
