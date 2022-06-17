using CapstoneOnGoing.Services.Interfaces;
using Models.Models;
using Repository.Interfaces;
using System;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Models.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using CapstoneOnGoing.Enums;
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

        public IEnumerable<GetApplicationDTO> GetAllApplication()
        {
            IEnumerable<Application> applications = _unitOfWork.Applications.GetAllApplicationsWithTeamTopicProject();

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
            string operation = request.Operation.ToLower();
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

                        //Create new project
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
