using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CapstoneOnGoing.Filter;
using CapstoneOnGoing.Helper;
using CapstoneOnGoing.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Models.Dtos;
using Models.Models;
using Models.Response;

namespace CapstoneOnGoing.Controllers
{
    [Route("api/v1/projects")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IProjectService _projectService;
        private readonly IUriService _uriService;
        private readonly ITeamService _teamService;
        private readonly IUserService _userService;

        public ProjectController(IMapper mapper, IProjectService projectService, IUriService uriService, ITeamService teamService, IUserService userService)
        {
            _mapper = mapper;
            _projectService = projectService;
            _uriService = uriService;
            _teamService = teamService;
            _userService = userService;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(GetProjectDetailResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenericResponse), StatusCodes.Status400BadRequest)]
        public IActionResult GetProjectById(Guid id)
        {
            GetProjectDetailResponse projectResponse = new GetProjectDetailResponse();

            GetProjectDetailDTO projectDetailDto = _projectService.GetProjectDetailById(id);
            GetTopicsResponse topicsResponse = _mapper.Map<GetTopicsResponse>(projectDetailDto.Topics);
            GetTeamDetailResponse teamDetailResponse = projectDetailDto.TeamDetailResponse;

            projectResponse.TopicsResponse = topicsResponse;
            projectResponse.TeamDetailResponse = teamDetailResponse;

            return Ok(projectResponse);
        }

        [HttpGet]
        public IActionResult GetAllProjects([FromQuery] PaginationFilter paginationFilter)
        {
            string route = Request.Path.Value;
            PaginationFilter validFilter;
            if (string.IsNullOrEmpty(paginationFilter.SearchString) ||
                string.IsNullOrWhiteSpace(paginationFilter.SearchString))
            {
                validFilter =
                    new PaginationFilter(String.Empty, paginationFilter.PageNumber, paginationFilter.PageSize);
            }
            else
            {
                validFilter =
                    new PaginationFilter(paginationFilter.SearchString, paginationFilter.PageNumber, paginationFilter.PageSize);
            }

            List<GetAllProjectDetailResponse> projectDetailResponses = new List<GetAllProjectDetailResponse>();

            IEnumerable<GetAllProjectsDetailDTO> projectsDetailDtos = _projectService.GetAllProjectResponse(validFilter, out int totalRecords);
            if (projectsDetailDtos != null)
            {
                Array.ForEach(projectsDetailDtos.ToArray(), projectsDetailDto =>
                {
                    GetAllProjectDetailResponse allProjectDetailResponse = new GetAllProjectDetailResponse();
                    GetTeamDetailResponse teamDetail =
                        _teamService.GetTeamDetail(projectsDetailDto.TeamDetailResponse.TeamId);

                    allProjectDetailResponse.TeamDetailResponse = teamDetail;

                    GetTopicAllProjectResponse topicAllProjectResponse = new GetTopicAllProjectResponse();
                    topicAllProjectResponse.TopicId = projectsDetailDto.TopicsAllProjectDto.TopicId;
                    topicAllProjectResponse.Description = projectsDetailDto.TopicsAllProjectDto.Description;
                    topicAllProjectResponse.Name = projectsDetailDto.TopicsAllProjectDto.Name;
                    List<GetLecturerResponse> lecturerResponses = new List<GetLecturerResponse>();
                    if (projectsDetailDto.TopicsAllProjectDto.LecturerIds is not null)
                    {
                        Array.ForEach(projectsDetailDto.TopicsAllProjectDto.LecturerIds.ToArray(), lecturerId =>
                        {
                            User lecturerUser = _userService.GetUserWithRoleById(lecturerId);
                            if (lecturerUser is not null)
                            {
                                GetLecturerResponse lecturerResponse = _mapper.Map<GetLecturerResponse>(lecturerUser);
                                lecturerResponses.Add(lecturerResponse);
                            }
                        });
                        topicAllProjectResponse.Lecturer = lecturerResponses;
                    }
                    else
                    {
                        topicAllProjectResponse.Lecturer = null;
                    }

                    Guid companyId;
                    if (projectsDetailDto.TopicsAllProjectDto.CompanyId is null)
                    {
                        companyId = Guid.Empty;
                    }
                    else
                    {
                        companyId = (Guid)projectsDetailDto.TopicsAllProjectDto.CompanyId;
                    }

                    User company = _userService.GetUserWithRoleById(companyId);
                        topicAllProjectResponse.Company = _mapper.Map<GetUserResponse>(company);


                        allProjectDetailResponse.TopicsResponse = topicAllProjectResponse;
                    projectDetailResponses.Add(allProjectDetailResponse);
                });
            }

            if (projectDetailResponses.Any())
            {
                PagedResponse<IEnumerable<GetAllProjectDetailResponse>> pagedResponse =
                    PaginationHelper<GetAllProjectDetailResponse>.CreatePagedResponse(projectDetailResponses, validFilter,
                        totalRecords, _uriService, route);
                return Ok(pagedResponse);
            }
            else
            {
                return Ok(projectDetailResponses);
            }
        }
    }
}
