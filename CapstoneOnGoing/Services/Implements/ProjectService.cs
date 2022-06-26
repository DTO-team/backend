using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CapstoneOnGoing.Enums;
using CapstoneOnGoing.Filter;
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
        private readonly ITeamService _teamService;

        public ProjectService(IUnitOfWork unitOfWork, IMapper mapper, ITeamService teamService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _teamService = teamService;
        }

        public IEnumerable<GetAllProjectsDetailDTO> GetAllProjectResponse(PaginationFilter validFilter, out int totalRecords)
        {
            IEnumerable<Project> projects = _unitOfWork.Project
                .GetAllProjectWithMentorTeamAndTeamStudents(validFilter.SearchString, validFilter.PageNumber, validFilter.PageSize, out totalRecords);

            List<GetAllProjectsDetailDTO> allProjectsDetailDetailDtos = new List<GetAllProjectsDetailDTO>();

            if (projects.Any())
            {
                GetAllProjectsDetailDTO allProjectDetailDto;
                Array.ForEach(projects.ToArray(), project =>
                {
                    allProjectDetailDto = new GetAllProjectsDetailDTO();
                    Application projectApplication = _unitOfWork.Applications.GetById(project.ApplicationId);
                    Topic projectTopic = _unitOfWork.Topic.GetById(projectApplication.TopicId);

                    GetTopicAllProjectDTO getTopicsAllProjectDto = new GetTopicAllProjectDTO()
                    { TopicId = projectTopic.Id, Name = projectTopic.Name, Description = projectTopic.Description, CompanyId = projectTopic.CompanyId };

                    allProjectDetailDto.TopicsAllProjectDto = getTopicsAllProjectDto;
                    allProjectDetailDto.TeamDetailResponse = _teamService.GetTeamDetail(project.TeamId);
                    allProjectsDetailDetailDtos.Add(allProjectDetailDto);
                });
                return allProjectsDetailDetailDtos;
            }
            return new List<GetAllProjectsDetailDTO>();
        }

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
