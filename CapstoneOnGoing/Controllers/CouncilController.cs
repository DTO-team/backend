using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CapstoneOnGoing.Logger;
using CapstoneOnGoing.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Models.Dtos;
using Models.Models;
using Models.Request;
using Models.Response;
using Newtonsoft.Json;
using Repository.Interfaces;

namespace CapstoneOnGoing.Controllers
{
	[Route("api/v1/councils")]
	[ApiController]
	public class CouncilController : ControllerBase
	{
		private readonly ILoggerManager _logger;
		private readonly IMapper _mapper;
		private readonly ICouncilService _councilService;
        private readonly IProjectService _projectService;
        private readonly ITeamService _teamService;
        private readonly IUserService _userService;

        public CouncilController(ILoggerManager logger, IMapper mapper, ICouncilService councilService, IProjectService projectService, ITeamService teamService, IUserService userService)
        {
            _logger = logger;
            _mapper = mapper;
            _councilService = councilService;
            _projectService = projectService;
            _teamService = teamService;
            _userService = userService;
        }

        [Authorize(Roles = "ADMIN,STUDENT,LECTURER")]
        [HttpGet("{councilId}/projects")]
        [ProducesResponseType(typeof(IEnumerable<GetAllProjectDetailResponse>), StatusCodes.Status200OK)]
        public IActionResult GetAllCouncilProjects(Guid councilId)
		{
			var headers = Request.Headers;
			StringValues CurrentSemester;
			if (!headers.Keys.Contains("currentsemester") || !headers.TryGetValue("currentsemester", out CurrentSemester))
			{
				_logger.LogWarn($"Controller: {nameof(TeamController)},Method: {nameof(GetAllCouncilProjects)}: Semester {CurrentSemester}");
				return BadRequest(new GenericResponse()
				{
					HttpStatus = StatusCodes.Status400BadRequest,
					Message = "Request does not have semester",
					TimeStamp = DateTime.Now
				});
			}

			GetSemesterDTO semesterDto = JsonConvert.DeserializeObject<GetSemesterDTO>(CurrentSemester.ToString());
			IEnumerable<GetProjectDetailDTO> allCouncilProject = _projectService.GetAllCouncilProject(councilId, semesterDto);
			List<GetAllProjectDetailResponse> allCouncilProjectResponse = new List<GetAllProjectDetailResponse>();
            if (allCouncilProject.Any())
            {
                foreach (GetProjectDetailDTO councilGetProjectDetailDto in allCouncilProject)
                {
                    GetAllProjectDetailResponse allProjectDetailResponse = new GetAllProjectDetailResponse();
                    GetProjectDetailDTO projectDetailDto =
                        _projectService.GetProjectDetailById(councilGetProjectDetailDto.ProjectId, semesterDto);
                    if (projectDetailDto is not null)
                    {
                        GetTopicAllProjectResponse topicsResponse =
                            _mapper.Map<GetTopicAllProjectResponse>(projectDetailDto.Topics);
                        List<GetLecturerResponse> lecturerResponses = new List<GetLecturerResponse>();


                        if (projectDetailDto.Topics.LecturerIds.Any())
                        {
                            Array.ForEach(projectDetailDto.Topics.LecturerIds.ToArray(), lecturerId =>
                            {
                                User lecturerUser = _userService.GetUserWithRoleById(lecturerId);
                                if (lecturerUser is not null)
                                {
                                    GetLecturerResponse lecturerResponse =
                                        _mapper.Map<GetLecturerResponse>(lecturerUser);
                                    lecturerResponses.Add(lecturerResponse);
                                }
                            });
                        }

                        User company = _userService.GetUserWithRoleById(projectDetailDto.Topics.CompanyId);
                        GetCompanyDTO companyDto = _mapper.Map<GetCompanyDTO>(company);
                        GetCompanyResponse companyResponse = _mapper.Map<GetCompanyResponse>(companyDto);

                        GetTeamDetailResponse teamDetailResponse = projectDetailDto.TeamDetailResponse;

                        allProjectDetailResponse.ProjectId = projectDetailDto.ProjectId;
                        allProjectDetailResponse.TopicsResponse = topicsResponse;
                        allProjectDetailResponse.TopicsResponse.LecturersDetails = lecturerResponses;
                        allProjectDetailResponse.TopicsResponse.CompanyDetail = companyResponse;
                        allProjectDetailResponse.TeamDetailResponse = teamDetailResponse;

                        allCouncilProjectResponse.Add(allProjectDetailResponse);
                    }
                }
                return Ok(allCouncilProjectResponse);
            }
			else
			{
                _logger.LogWarn($"Controller: {nameof(TeamController)},Method: {nameof(GetAllCouncilProjects)}: Semester {CurrentSemester}");
                return BadRequest(new GenericResponse()
                {
                    HttpStatus = StatusCodes.Status400BadRequest,
                    Message = "Request does not have semester",
                    TimeStamp = DateTime.Now
                });
			}
		}

		[Authorize(Roles = "ADMIN")]
		[HttpGet]
		[ProducesResponseType(typeof(IEnumerable<GetCouncilResponse>),StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(GenericResponse),StatusCodes.Status400BadRequest)]
		public IActionResult GetAllCouncils()
		{
			var headers = Request.Headers;
			StringValues CurrentSemester;
			if (!headers.Keys.Contains("currentsemester") || !headers.TryGetValue("currentsemester", out CurrentSemester))
			{
				_logger.LogWarn($"Controller: {nameof(TeamController)},Method: {nameof(GetAllCouncils)}: Semester {CurrentSemester}");
				return BadRequest(new GenericResponse()
				{
					HttpStatus = StatusCodes.Status400BadRequest,
					Message = "Request does not have semester",
					TimeStamp = DateTime.Now
				});
			}

			GetSemesterDTO semesterDto = JsonConvert.DeserializeObject<GetSemesterDTO>(CurrentSemester.ToString());
			IEnumerable<GetCouncilResponse> councilResponses = _councilService.GetAllCouncils(semesterDto);
			return Ok(councilResponses);
		}

        // [Authorize(Roles = "ADMIN,LECTURER,STUDENT")]
        [HttpGet("lecturer/{lecturerId}")]
        [ProducesResponseType(typeof(IEnumerable<Guid>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenericResponse), StatusCodes.Status400BadRequest)]
        public IActionResult GetCouncilIdByLecturerId(Guid lecturerId)
        {
            IEnumerable<Guid> councilIds = _councilService.GetCouncilIdByLecturerId(lecturerId);
            return Ok(councilIds);
        }

		[Authorize(Roles = "ADMIN")]
		[HttpGet("{id}")]
		[ProducesResponseType(typeof(GetCouncilResponse), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(GenericResponse), StatusCodes.Status400BadRequest)]
		public IActionResult GetCouncilById(Guid id)
		{
			var headers = Request.Headers;
			StringValues CurrentSemester;
			if (!headers.Keys.Contains("currentsemester") || !headers.TryGetValue("currentsemester", out CurrentSemester))
			{
				_logger.LogWarn($"Controller: {nameof(TeamController)},Method: {nameof(GetAllCouncils)}: Semester {CurrentSemester}");
				return BadRequest(new GenericResponse()
				{
					HttpStatus = StatusCodes.Status400BadRequest,
					Message = "Request does not have semester",
					TimeStamp = DateTime.Now
				});
			}

			GetSemesterDTO semesterDto = JsonConvert.DeserializeObject<GetSemesterDTO>(CurrentSemester.ToString());
			GetCouncilResponse councilResponse = _councilService.GetCouncilById(id, semesterDto);
			return Ok(councilResponse);
		}

		[Authorize(Roles = "ADMIN")]
		[HttpPut("{id}")]
		public IActionResult UpdateCouncil(Guid id, UpdateCouncilRequest updateCouncilRequest)
		{
			bool isSuccessful = _councilService.UpdateCouncil(id, updateCouncilRequest);
			if (!isSuccessful)
			{
				return BadRequest(new GenericResponse()
				{
					HttpStatus = StatusCodes.Status400BadRequest,
					Message = "Update council failed",
					TimeStamp = DateTime.Now
				});
			}
			return Ok(new GenericResponse()
			{
				HttpStatus = StatusCodes.Status400BadRequest,
				Message = "Update council successfully",
				TimeStamp = DateTime.Now
			});
		}
	}
}
