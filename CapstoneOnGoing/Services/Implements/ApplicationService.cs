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
            throw new NotImplementedException();
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
                            _unitOfWork.TopicLecturer.Get(lecturer => lecturer.Id == application.TopicId).Select(x => x.LecturerId).FirstOrDefault();
                        Lecturer lecturer = _unitOfWork.Lecturer.Get(lecturer => lecturer.Id == topicLecturerId)
                            .FirstOrDefault();

                        project.Mentors = new List<Mentor>()
                        {
                            new Mentor()
                            {
                                ProjectId = project.Id,
                                Lecturer = lecturer,
                                LecturerId = topicLecturerId,
                            }
                        };

                        _unitOfWork.Project.Insert(project);
                        _unitOfWork.Save();
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
