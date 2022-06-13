using CapstoneOnGoing.Services.Interfaces;
using Models.Models;
using Repository.Interfaces;
using System;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Models.Dtos;
using Models.Response;

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

        public GetApplicationResponse GetApplicationById(Guid id)
        {
            Application application = _unitOfWork.Applications.GetApplicationWithTeamTopicProject(id);

            if (application != null)
            {
                GetApplicationResponse response = new GetApplicationResponse();
                GetApplicationDTO applicationDto = _mapper.Map<GetApplicationDTO>(application);

                //Team's information
                ResponseApplyTeamFields applicationFields = new ResponseApplyTeamFields();
                applicationFields.LeaderName = _unitOfWork.User.GetById(applicationDto.TeamInformation.TeamLeaderId).FullName;
                applicationFields.TeamName = applicationDto.TeamInformation.TeamName;
                applicationFields.TeamSemesterSeason = _unitOfWork.Semester.GetById(applicationDto.TeamInformation.TeamSemesterId).Season;
                response.ApplyTeam = applicationFields;

                //Team's topic information
                ResponseTopicFields topicFields = new ResponseTopicFields();
                Topic teamTopic = _unitOfWork.Topic.GetById(applicationDto.Topic.TopicId);
                topicFields.TopicName = teamTopic.Name;

                string topicDescription = applicationDto.Topic.Description;
                if (!string.IsNullOrEmpty(topicDescription))
                {
                    topicFields.Description = teamTopic.Description;
                }
                else
                {
                    topicFields.Description = "";
                }
                response.Topic = topicFields;


                //Team's application status
                int teamStatus = applicationDto.Status;

                if (teamStatus.Equals(1))
                {
                    response.Status = "PENDING";
                }
                else if (teamStatus.Equals(2))
                {
                    response.Status = "APPROVED";
                }
                else
                {
                    response.Status = "REJECTED";
                }

                return response;
            }
            else
            {
                throw new BadHttpRequestException("Application not existed");
            }
        }
    }
}
