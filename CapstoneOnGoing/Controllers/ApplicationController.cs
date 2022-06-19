using CapstoneOnGoing.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CapstoneOnGoing.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Models.Dtos;
using Models.Models;
using Models.Request;
using Models.Response;

namespace CapstoneOnGoing.Controllers
{
    [Route("api/v1/applications")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {

        private readonly IApplicationService _applicationService;
        private readonly IStudentService _studentService;
        private readonly ISemesterService _semesterService;
        private readonly ITopicService _topicService;
        private readonly IUriService _uriService;
        private readonly IMapper _mapper;

        public ApplicationController(IApplicationService applicationService, IStudentService studentService, ISemesterService semesterService, ITopicService topicService, IUriService uriService, IMapper mapper)
        {
            _applicationService = applicationService;
            _studentService = studentService;
            _semesterService = semesterService;
            _topicService = topicService;
            _uriService = uriService;
            _mapper = mapper;
        }

        private GetApplicationResponse MappingToApplicationResponseFromDto(GetApplicationDTO applicationDto)
        {
            GetApplicationResponse response = new GetApplicationResponse();

            response.ApplicationId = applicationDto.ApplicationId;

            //Team's information
            ResponseApplyTeamFields applicationFields = new ResponseApplyTeamFields();
            applicationFields.TeamId = applicationDto.TeamInformation.TeamId;

            //Get leader student information
            User leaderStudent = _studentService.GetStudentById(applicationDto.TeamInformation.TeamLeaderId);
            applicationFields.LeaderStudent = _mapper.Map<StudentResponse>(leaderStudent);
            applicationFields.TeamName = applicationDto.TeamInformation.TeamName;
            applicationFields.TeamSemesterSeason = _semesterService.GetSemesterById(applicationDto.TeamInformation.TeamSemesterId).Season;
            response.ApplyTeam = applicationFields;

            //Team's topic information
            ResponseTopicFields topicFields = new ResponseTopicFields();
            Topic teamTopic = _topicService.GetTopicById(applicationDto.Topic.TopicId);
            topicFields.TopicId = teamTopic.Id;
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

            return response;
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN,STUDENT")]
        [ProducesResponseType(typeof(GetApplicationResponse), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(GenericResponse), StatusCodes.Status400BadRequest)]
        public IActionResult CreateNewApplication(CreateNewApplicationRequest newApplicationRequest)
        {
            GetApplicationResponse response;
            GetApplicationDTO applicationDto = _applicationService.CreateNewApplication(newApplicationRequest);
            if (applicationDto is not null)
            {
                response = MappingToApplicationResponseFromDto(applicationDto);

                return CreatedAtAction("CreateNewApplication", response);
            }
            else
            {
                return BadRequest(new GenericResponse()
                { HttpStatus = 400, Message = "Create New Application failed", TimeStamp = DateTime.Now });
            }
        }

        [HttpGet]
        // [Authorize(Roles = "ADMIN,LECTURER,STUDENT")]
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
                    GetApplicationResponse response = MappingToApplicationResponseFromDto(applicationDto);
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
        // [Authorize(Roles = "ADMIN")]
        [ProducesResponseType(typeof(GetApplicationResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenericResponse), StatusCodes.Status400BadRequest)]
        public IActionResult GetApplicationById(Guid id)
        {
            GetApplicationResponse response;
            GetApplicationDTO applicationDto = _applicationService.GetApplicationById(id);
            if (applicationDto != null)
            {
                response = MappingToApplicationResponseFromDto(applicationDto);
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

        [HttpDelete("status")]
        [Authorize(Roles = "ADMIN")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenericResponse), StatusCodes.Status400BadRequest)]
        public IActionResult DeleteApplication([FromQuery] Guid id)
        {
            bool isSuccess = _applicationService.UpdateApplicationStatusById(id, new UpdateApplicationStatusRequest() {Op = "delete"});
            if (isSuccess)
            {
                return Ok();
            }
            else
            {
                GenericResponse errorResponse = new GenericResponse()
                    { HttpStatus = 400, Message = "Delete application failed!", TimeStamp = DateTime.Now };
                return BadRequest(errorResponse);
            }
        }

    }
}
