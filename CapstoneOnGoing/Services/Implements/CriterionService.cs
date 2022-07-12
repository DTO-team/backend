using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CapstoneOnGoing.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Models.Dtos;
using Models.Models;
using Repository.Interfaces;

namespace CapstoneOnGoing.Services.Implements
{
    public class CriterionService : ICriterionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CriterionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IEnumerable<CriterionDTO> GetAllCriterion()
        {
            IEnumerable<Criterion> criterions = _unitOfWork.Criteria.Get(null, null, "Grades,Questions");
            IEnumerable<CriterionDTO> criterionDtos;

            if (criterions.Any())
            {
                criterionDtos = _mapper.Map<IEnumerable<CriterionDTO>>(criterions);
                return criterionDtos;
            }
            else
            {
                throw new BadHttpRequestException("Load criterion failed!");
            }
        }

        public CriterionDTO GetCriterionById(Guid criterionId)
        {
            Criterion criterion = _unitOfWork.Criteria.Get(null, null, "Grades,Questions").FirstOrDefault();
            CriterionDTO criterionDto;

            if (criterion is not null)
            {
                criterionDto = _mapper.Map<CriterionDTO>(criterion);
                return criterionDto;
            }
            else
            {
                throw new BadHttpRequestException($"Criterion with {criterionId} id is not existed!");
            }
        }
    }
}
