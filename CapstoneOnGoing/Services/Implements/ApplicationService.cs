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
    }
}
