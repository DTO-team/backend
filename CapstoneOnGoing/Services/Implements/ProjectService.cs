using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CapstoneOnGoing.Enums;
using CapstoneOnGoing.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Models.Dtos;
using Models.Models;
using Models.Response;
using Repository.Interfaces;

namespace CapstoneOnGoing.Services.Implements
{
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ITeamService _teamService;
        private readonly ITopicService _topicService;
        private readonly IApplicationService _applicationService;

        public ProjectService(IUnitOfWork unitOfWork, IMapper mapper, ITeamService teamService, ITopicService topicService, IApplicationService applicationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _teamService = teamService;
            _topicService = topicService;
            _applicationService = applicationService;
        }

        public IEnumerable<GetProjectResponse> GetAllProjectResponse()
        {
            return new List<GetProjectResponse>();
        }

        public GetProjectResponse GetProjectDetailById(Guid projectId)
        {
            GetProjectResponse projectResponse = new GetProjectResponse();
            Project project = _unitOfWork.Project.GetById(projectId);

            GetApplicationDTO applicationById = _applicationService.GetApplicationById(project.ApplicationId);

            Guid teamId = applicationById.TeamInformation.TeamId;
                GetTeamDetailResponse projectTeamDetail = _teamService.GetTeamDetail(teamId);
                if (projectTeamDetail is not null)
                {
                    projectResponse.TeamDetail = projectTeamDetail;
                }
                else
                {
                    throw new BadHttpRequestException($"Team with {teamId} id is not existed!");
                }

                Guid topicId = applicationById.Topic.TopicId;
                Semester currentSemester = _unitOfWork.Semester.Get(x => x.Status == (int)SemesterStatus.Preparing).FirstOrDefault();
                Topic topic = _unitOfWork.Topic.Get(x => (x.SemesterId == currentSemester.Id),
                        null,
                        "TopicLecturers")
                    .FirstOrDefault();

                GetTopicsDTO topicsDto = 


        }
    }
}
