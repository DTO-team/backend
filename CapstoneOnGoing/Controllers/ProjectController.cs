﻿using Microsoft.AspNetCore.Mvc;
using System;
using AutoMapper;
using CapstoneOnGoing.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Models.Dtos;
using Models.Response;

namespace CapstoneOnGoing.Controllers
{
    [Route("api/v1/projects")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IProjectService _projectService;

        public ProjectController(IMapper mapper, IProjectService projectService)
        {
            _mapper = mapper;
            _projectService = projectService;
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
    }
}
