using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CapstoneOnGoing.Enums;
using CapstoneOnGoing.Services.Interfaces;
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
        private readonly IApplicationService _applicationService;
        private readonly ILecturerService _lecturerService;
        private readonly ITeamService _teamService;

        public ProjectService(IUnitOfWork unitOfWork, IMapper mapper, IApplicationService applicationService, ILecturerService lecturerService, ITeamService teamService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _applicationService = applicationService;
            _lecturerService = lecturerService;
            _teamService = teamService;
        }

        // public IEnumerable<GetProjectResponse> GetAllProjectResponse()
        // {
        //     return new List<GetProjectResponse>();
        // }

        public GetProjectDetailDTO GetProjectDetailById(Guid projectId)
        {
            GetProjectDetailDTO projectDto = new GetProjectDetailDTO();
            Project project = _unitOfWork.Project.GetById(projectId);

            Semester currentSemester = _unitOfWork.Semester.Get(x => x.Status == (int)SemesterStatus.Preparing).FirstOrDefault();

            Topic topic = _unitOfWork.Topic.Get(x => (x.SemesterId == currentSemester.Id ), null, "TopicLecturers").FirstOrDefault();

            GetTopicsDTO topicDto = _mapper.Map<GetTopicsDTO>(topic);
            projectDto.Topics = topicDto;


            GetTeamDetailResponse teamDetailResponse = _teamService.GetTeamDetail(project.TeamId);

            projectDto.TeamDetailResponse = teamDetailResponse;

            return projectDto;
        }
    }
}
