using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CapstoneOnGoing.Enums;
using CapstoneOnGoing.Filter;
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

        public ProjectService(IUnitOfWork unitOfWork, IMapper mapper, ITeamService teamService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _teamService = teamService;
        }

        public IEnumerable<GetAllProjectsDetailDTO> GetAllProjectResponse(string SearchString)
        {
            IEnumerable<Project> projects = _unitOfWork.Project
                .GetAllProjectWithMentorTeamAndTeamStudents(SearchString);

            List<GetAllProjectsDetailDTO> allProjectsDetailDetailDtos = new List<GetAllProjectsDetailDTO>();

            if (projects.Any())
            {
                GetAllProjectsDetailDTO allProjectDetailDto;
                Array.ForEach(projects.ToArray(), project =>
                {
                    allProjectDetailDto = new GetAllProjectsDetailDTO();
                    allProjectDetailDto.ProjectId = project.Id;
                    Application projectApplication = _unitOfWork.Applications.GetById(project.ApplicationId);
                    Topic projectTopic = _unitOfWork.Topic.GetById(projectApplication.TopicId);
                    IEnumerable<TopicLecturer> teamStudents =
                        _unitOfWork.TopicLecturer.Get(topicLecturer => topicLecturer.TopicId.Equals(projectTopic.Id));

                    List<Guid> lecturerIds = new List<Guid>();
                    Array.ForEach(teamStudents.ToArray(), teamStudent =>
                    {
                        lecturerIds.Add(teamStudent.LecturerId);
                    });

                    GetTopicAllProjectDTO getTopicsAllProjectDto = new GetTopicAllProjectDTO()
                    { TopicId = projectTopic.Id, Name = projectTopic.Name, Description = projectTopic.Description, CompanyId = projectTopic.CompanyId, LecturerIds = lecturerIds};

                    allProjectDetailDto.TopicsAllProjectDto = getTopicsAllProjectDto;
                    allProjectDetailDto.TeamDetailResponse = _teamService.GetTeamDetail(project.TeamId);
                    allProjectsDetailDetailDtos.Add(allProjectDetailDto);
                });
                return allProjectsDetailDetailDtos;
            }
            return new List<GetAllProjectsDetailDTO>();
        }

        public GetProjectDetailDTO GetProjectDetailById(Guid projectId, GetSemesterDTO semester)
        {
            GetProjectDetailDTO projectDto = new GetProjectDetailDTO();
            Project project = _unitOfWork.Project.GetById(projectId);

            if (project is not null)
            {
                projectDto.ProjectId = project.Id;
                GetSemesterDTO currentSemester = semester;

                Application projectApplication =
                    _unitOfWork.Applications.GetApplicationWithTeamTopicProject(project.ApplicationId);

                Topic topic = _unitOfWork.Topic.Get(x => (x.SemesterId == currentSemester.Id && x.Id.Equals(projectApplication.TopicId)), null, "TopicLecturers").FirstOrDefault();
                // Topic topic = _unitOfWork.Topic.Get(x => (x.Id.Equals(projectApplication.Id)), null, "TopicLecturers").FirstOrDefault();

                GetTopicsDTO topicDto = _mapper.Map<GetTopicsDTO>(topic);
                projectDto.Topics = topicDto;

                IEnumerable<TopicLecturer> topicLecturers =
                    _unitOfWork.TopicLecturer.Get(topicLecturer => topicLecturer.TopicId.Equals(topic.Id));

                List<Guid> lecturerIds = new List<Guid>();
                Array.ForEach(topicLecturers.ToArray(), topicLecturer =>
                {
                    lecturerIds.Add(topicLecturer.LecturerId);
                });

                GetTeamDetailResponse teamDetailResponse = _teamService.GetTeamDetail(project.TeamId);

                projectDto.Topics.LecturerIds = lecturerIds;
                projectDto.TeamDetailResponse = teamDetailResponse;
            }
            else
            {
                throw new BadHttpRequestException($"Project with {projectId} id is not existed !");
            }
            return projectDto;
        }

        public IEnumerable<GetProjectDetailDTO> GetAllCouncilProject(Guid councilId, GetSemesterDTO semester)
        {
            Council council = _unitOfWork.Councils.GetCouncilWithProjectAndTeamById(councilId);

            List<GetProjectDetailDTO> allProjectsDetailDtos = new List<GetProjectDetailDTO>();

            if (council is not null)
            {
                IEnumerable<CouncilProject> councilProjects = council.CouncilProjects;

                foreach (CouncilProject councilProject in councilProjects)
                {
                    GetProjectDetailDTO projectDetailDto = GetProjectDetailById(councilProject.ProjectId, semester);
                    if (projectDetailDto is not null)
                    {
                        allProjectsDetailDtos.Add(projectDetailDto);
                    }
                }

                return allProjectsDetailDtos;
            }
            else
            {
                throw new BadHttpRequestException($"Council with {councilId} id is not existed!");
            }
        }
    }
}
