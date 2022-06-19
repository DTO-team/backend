using CapstoneOnGoing.Services.Interfaces;
using Models.Models;
using Repository.Interfaces;
using System;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Models.Dtos;
using System.Collections.Generic;
using System.Linq;
using CapstoneOnGoing.Enums;
using CapstoneOnGoing.Filter;
using Models.Request;

namespace CapstoneOnGoing.Services.Implements
{
    public class ApplicationService : IApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ApplicationService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public GetApplicationDTO CreateNewApplication(CreateNewApplicationRequest newApplicationRequest)
        {
            GetApplicationDTO applicationDto;
            Guid requestTeamId = newApplicationRequest.TeamId;
            Guid requestTopicId = newApplicationRequest.TopicId;
            Application existApplication =
                _unitOfWork.Applications.Get(app => (app.TeamId == requestTeamId && app.TopicId == requestTopicId)).FirstOrDefault();

            //Check topic and team is existed
            Topic existTopic = _unitOfWork.Topic.GetById(requestTopicId);
            Team existTeam = _unitOfWork.Team.GetById(requestTeamId);
            if (existTeam is null)
            {
                throw new BadHttpRequestException(
                    $"Team with {requestTeamId} teamId is not existed");
            }
            if (existTopic is null)
            {
                throw new BadHttpRequestException(
                    $"Topic with {requestTopicId} topicId is not existed");
            }

            //Create new application
            if (existApplication is null)
            {
                //Create application
                Application application = new Application();
                application.TeamId = requestTeamId;
                application.TopicId = requestTopicId;
                application.StatusId = (int)ApplicationStatus.Pending;

                _unitOfWork.Applications.Insert(application);
                _unitOfWork.Save();
            }
            else
            {
                throw new BadHttpRequestException(
                    $"Application with teamId: {requestTeamId} and topicId: {requestTopicId} is existed");
            }

            //Get created application
            Application newCreatedApplication =
                _unitOfWork.Applications.GetApplicationWithTeamTopicProjectByTeamIdAndTopicId(newApplicationRequest);

            applicationDto = _mapper.Map<GetApplicationDTO>(newCreatedApplication);

            return applicationDto;
        }

        public IEnumerable<GetApplicationDTO> GetAllApplications(PaginationFilter validFilter, out int totalRecords)
        {
            IEnumerable<Application> applications = _unitOfWork.Applications.GetAllApplicationsWithTeamTopicProject(validFilter.SearchString,validFilter.PageNumber,validFilter.PageSize,out totalRecords);

            if (applications.Any())
            {
                IEnumerable<GetApplicationDTO> applicationDtos =
                    _mapper.Map<IEnumerable<GetApplicationDTO>>(applications);
                return applicationDtos;
            }

            return null;
        }

        public GetApplicationDTO GetApplicationById(Guid id)
        {
            Application application = _unitOfWork.Applications.GetApplicationWithTeamTopicProject(id);

            if (application != null)
            {
                GetApplicationDTO applicationDto = _mapper.Map<GetApplicationDTO>(application);

                return applicationDto;
            }
            else
            {
                throw new BadHttpRequestException("Application not existed");
            }
        }

        public bool UpdateApplicationStatusById(Guid id, UpdateApplicationStatusRequest request)
        {
            bool isSuccess = false;
            Application application = _unitOfWork.Applications.GetById(id);
            string operation = request.Op.ToLower();
            if (application != null)
            {
                switch (operation)
                {
                    case "reject":
                        application.StatusId = (int)ApplicationStatus.Rejected;
                        isSuccess = true;
                        _unitOfWork.Applications.Update(application);
                        _unitOfWork.Save();
                        break;

                    case "approve":
                        //Change status of application to approve
                        application.StatusId = (int)ApplicationStatus.Approved;
                        _unitOfWork.Applications.Update(application);

                        //Get all ANOTHER application with the same topic to reject
                        IEnumerable<Application> sameTopicApplications = _unitOfWork.Applications.Get(app =>
                            (app.TopicId == application.TopicId && app.Id != application.Id && app.StatusId != (int)ApplicationStatus.Deleted));
                        foreach (Application sameTopicApp in sameTopicApplications)
                        {
                            sameTopicApp.StatusId = (int)ApplicationStatus.Rejected;
                            _unitOfWork.Applications.Update(sameTopicApp);
                        }

                        //Create new project with the application's topic after approve 1 application
                        Project project = new Project();
                        if (application.TeamId == Guid.Empty)
                        {
                            throw new BadHttpRequestException("Team Id is required to create new Project");
                        }
                        project.ApplicationId = application.Id;
                        project.TeamId = application.TeamId;

                        Guid topicLecturerId =
                            _unitOfWork.TopicLecturer.Get(lecturer => lecturer.TopicId == application.TopicId).Select(x => x.LecturerId).FirstOrDefault();

                        project.Mentors = new List<Mentor>()
                        {
                            new Mentor()
                            {
                                ProjectId = project.Id,
                                LecturerId = topicLecturerId,
                            }
                        };

                        _unitOfWork.Project.Insert(project);
                        _unitOfWork.Save();
                        isSuccess = true;
                        break;

                    case "delete":
                        application.StatusId = (int)ApplicationStatus.Deleted;
                        isSuccess = true;
                        _unitOfWork.Applications.Update(application);
                        _unitOfWork.Save();
                        break;

                    default:
                        throw new BadHttpRequestException("Wrong operation request!");
                }
            }
            else
            {
                throw new BadHttpRequestException("Application not existed");
            }
            return isSuccess;
        }
    }
}
