using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CapstoneOnGoing.Enums;
using CapstoneOnGoing.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Models.Dtos;
using Models.Models;
using Models.Request;
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

        public IEnumerable<CriteriaDTO> GetAllCriteria()
        {
            IEnumerable<Criterion> criterions = _unitOfWork.Criteria.Get(null, null, "Grades,Questions");
            IEnumerable<CriteriaDTO> criterionDtos;

            if (criterions.Any())
            {
                criterionDtos = _mapper.Map<IEnumerable<CriteriaDTO>>(criterions);
                return criterionDtos;
            }
            else
            {
                throw new BadHttpRequestException("Load criterias failed!");
            }
        }

        public CriteriaDTO GetCriteriaById(Guid criteriaId)
        {
            Criterion criteria = _unitOfWork.Criteria.Get(criteria => criteria.Id.Equals(criteriaId), null, "Grades,Questions").FirstOrDefault();
            CriteriaDTO criteriaDto;

            if (criteria is not null)
            {
                criteriaDto = _mapper.Map<CriteriaDTO>(criteria);
                return criteriaDto;
            }
            else
            {
                throw new BadHttpRequestException($"Criteria with {criteriaId} id is not existed!");
            }
        }

        public CriteriaDTO GetCriteriaByCode(string criteriaCode)
        {
            if (!(string.IsNullOrEmpty(criteriaCode)))
            {
                Criterion criteria = _unitOfWork.Criteria.Get(criteria => criteria.Code.Equals(criteriaCode),null, "Grades,Questions").FirstOrDefault();
                if (criteria is not null)
                {
                    CriteriaDTO criteriaDto = _mapper.Map<CriteriaDTO>(criteria);
                    return criteriaDto;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                throw new BadHttpRequestException("criteria code is empty");
            }
        }

        public bool CreateNewCriteria(CreateCriteriaRequest newCriteriaRequest)
        {
            CriteriaDTO existedCriteriaDto = GetCriteriaByCode(newCriteriaRequest.Code);
            
            Array.ForEach(newCriteriaRequest.GradesRequest.ToArray(), gradeRequest =>
            {
                string gradeLevel = gradeRequest.Level.ToUpper();
                if (!(gradeLevel.Equals(GradeLevels.ACCEPTABLE.ToString())
                    || gradeLevel.Equals(GradeLevels.EXCELLENT.ToString())
                    || gradeLevel.Equals(GradeLevels.GOOD.ToString())
                    || gradeLevel.Equals(GradeLevels.FAIL.ToString())))
                {
                    throw new BadHttpRequestException("Grade level only have 4 type: ACCEPTABLE, EXCELLENT, GOOD, FAIL");
                }
            });

            if (existedCriteriaDto is null)
            {
                Criterion createCriteria = _mapper.Map<Criterion>(newCriteriaRequest);
                createCriteria.Grades = _mapper.Map<ICollection<Grade>>(newCriteriaRequest.GradesRequest);
                createCriteria.Questions = _mapper.Map<ICollection<Question>>(newCriteriaRequest.QuestionsRequest);
                _unitOfWork.Criteria.Insert(createCriteria);
                return _unitOfWork.Save() > 0;
            }
            else
            {
                throw new BadHttpRequestException($"Criteria with {newCriteriaRequest.Code} code is existed!");
            }
        }

    }
}
