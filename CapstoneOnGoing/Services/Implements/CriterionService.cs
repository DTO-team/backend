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

            if (!string.IsNullOrEmpty(newCriteriaRequest.Evaluation))
            {
                newCriteriaRequest.Evaluation = $"- {newCriteriaRequest.Evaluation}";
            }
            
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
                else
                {
                    gradeRequest.Level = gradeRequest.Level.ToUpper();
                }

                if (!gradeRequest.MaxPoint.Equals(0) && !gradeRequest.MinPoint.Equals(0))
                {
                    if (gradeRequest.MaxPoint < gradeRequest.MinPoint)
                    {
                        int minPoint = gradeRequest.MaxPoint;
                        gradeRequest.MaxPoint = gradeRequest.MinPoint;
                        gradeRequest.MinPoint = minPoint;
                    }
                }
                else
                {
                    throw new BadHttpRequestException("Max point or Min point is cannot be 0!");
                }
                if (gradeRequest.MaxPoint < gradeRequest.MinPoint)
                {
                    int minPoint = gradeRequest.MaxPoint;
                    gradeRequest.MaxPoint = gradeRequest.MinPoint;
                    gradeRequest.MinPoint = minPoint;
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

        public bool UpdateCriteria(Guid criteriaId, UpdateCriteriaRequest updateCriteriaRequest)
        {
            CriteriaDTO existedCriteria = GetCriteriaById(criteriaId);
            if (existedCriteria is not null)
            {
                if (string.IsNullOrEmpty(updateCriteriaRequest.Name))
                {
                    updateCriteriaRequest.Name = existedCriteria.Name;
                }
                if (string.IsNullOrEmpty(updateCriteriaRequest.Evaluation))
                {
                    updateCriteriaRequest.Evaluation = existedCriteria.Evaluation;
                }
                if (!string.IsNullOrEmpty(updateCriteriaRequest.Evaluation))
                {
                    updateCriteriaRequest.Evaluation = " - " + updateCriteriaRequest.Evaluation;
                }

                Criterion criterion = _mapper.Map<Criterion>(updateCriteriaRequest);
                criterion.Id = criteriaId;
                criterion.Code = existedCriteria.Code;
                if (updateCriteriaRequest.Grades.Count > 0)
                {
                    foreach (UpdateCriteriaGradeRequest updateCriteriaGradeRequest in updateCriteriaRequest.Grades)
                    {
                        Grade grade = _unitOfWork.Grade
                            .Get(grade => (grade.CriteriaId.Equals(criteriaId) && grade.Id.Equals(updateCriteriaGradeRequest.Id))).FirstOrDefault();
                        if (grade is not null)
                        {
                            string gradeLevel = updateCriteriaGradeRequest.Level.ToUpper();
                            if (!(gradeLevel.Equals(GradeLevels.ACCEPTABLE.ToString())
                                  || gradeLevel.Equals(GradeLevels.EXCELLENT.ToString())
                                  || gradeLevel.Equals(GradeLevels.GOOD.ToString())
                                  || gradeLevel.Equals(GradeLevels.FAIL.ToString())))
                            {
                                throw new BadHttpRequestException($"Grade level on grade with {updateCriteriaGradeRequest.Id} id only have 4 type: ACCEPTABLE, EXCELLENT, GOOD, FAIL");
                            }
                            else
                            {
                                gradeLevel = updateCriteriaGradeRequest.Level.ToUpper();
                            }

                            if (updateCriteriaGradeRequest.MinPoint.Equals(0) || updateCriteriaGradeRequest.MaxPoint.Equals(0))
                            {
                                updateCriteriaGradeRequest.MinPoint = grade.MinPoint;
                                updateCriteriaGradeRequest.MaxPoint = grade.MaxPoint;
                            }

                            if (updateCriteriaGradeRequest.MaxPoint < updateCriteriaGradeRequest.MinPoint)
                            {
                                int minPoint = updateCriteriaGradeRequest.MaxPoint;
                                updateCriteriaGradeRequest.MaxPoint = updateCriteriaGradeRequest.MinPoint;
                                updateCriteriaGradeRequest.MinPoint = minPoint;
                            }

                            Grade updateCriteriaGrade = _mapper.Map<Grade>(updateCriteriaGradeRequest);
                            updateCriteriaGrade.CriteriaId = criteriaId;
                            _unitOfWork.Grade.Update(updateCriteriaGrade);
                        }
                        else
                        {
                            throw new BadHttpRequestException(
                                $"Grade with {updateCriteriaGradeRequest.Id} id is not existed in the criteria!");
                        }
                    }
                }

                if (updateCriteriaRequest.Questions.Count > 0)
                {
                    foreach (UpdateCriteriaQuestionRequest updateCriteriaQuestionRequest in updateCriteriaRequest.Questions)
                    {
                        Question question = _unitOfWork.Question
                            .Get(question => (question.CriteriaId.Equals(criteriaId) && question.Id.Equals(updateCriteriaQuestionRequest.Id))).FirstOrDefault();
                        if (question is not null)
                        {
                            if (string.IsNullOrEmpty(updateCriteriaQuestionRequest.Description))
                            {
                                updateCriteriaQuestionRequest.Description = question.Description;
                            }

                            if (string.IsNullOrEmpty(updateCriteriaQuestionRequest.Priority))
                            {
                                updateCriteriaQuestionRequest.Priority = question.Priority;
                            }

                            if (string.IsNullOrEmpty(updateCriteriaQuestionRequest.SubCriteria))
                            {
                                updateCriteriaQuestionRequest.SubCriteria = question.SubCriteria;
                            }

                            Question updateQuestion = _mapper.Map<Question>(updateCriteriaQuestionRequest);
                            updateQuestion.CriteriaId = criteriaId;
                            _unitOfWork.Question.Update(updateQuestion);
                        }
                        else
                        {
                            throw new BadHttpRequestException(
                                $"Question with {updateCriteriaQuestionRequest.Id} criteria id is not existed in the criteria!"
                                );
                        }
                    }
                }
                _unitOfWork.Criteria.Update(criterion);
                return _unitOfWork.Save() > 0;
            }
            else
            {
                throw new BadHttpRequestException($"Criteria with {criteriaId} id is not existed");
            }
        }

        public bool DeleteCriteria(Guid criteriaId)
        {
            if (!criteriaId.Equals(Guid.Empty))
            {
                CriteriaDTO criteria = GetCriteriaById(criteriaId);

                if (criteria.Grades.Count > 0)
                {
                    IEnumerable<Grade> criteriaGrades =
                        _unitOfWork.Grade.Get(grade => grade.CriteriaId.Equals(criteriaId));

                    foreach (Grade deleteCriteriaGrade in criteriaGrades)
                    {
                        _unitOfWork.Grade.Delete(deleteCriteriaGrade);
                    }
                }

                if (criteria.Questions.Count > 0)
                {
                    IEnumerable<Question> criteriaQuestions =
                        _unitOfWork.Question.Get(question => question.CriteriaId.Equals(criteriaId));

                    foreach (Question deleteCriteriaQuestion in criteriaQuestions)
                    {
                        _unitOfWork.Question.Delete(deleteCriteriaQuestion);
                    }
                }

                Criterion deleteCriteria = _unitOfWork.Criteria.GetById(criteriaId);
                _unitOfWork.Criteria.Delete(deleteCriteria);

                return _unitOfWork.Save() > 0;
            }
            else
            {
                throw new BadHttpRequestException("Criteria Id to delete is empty!");
            }
        }

        public bool CreateNewCriteriaGrade(Guid criteriaId, CreateNewCriteriaGradeRequest newCriteriaGradeRequest)
        {
            Criterion criteria = _unitOfWork.Criteria.GetById(criteriaId);
            if (criteria is not null)
            {
                Grade newGrade = _mapper.Map<Grade>(newCriteriaGradeRequest);
                newGrade.CriteriaId = criteriaId;

                _unitOfWork.Grade.Insert(newGrade);
                return _unitOfWork.Save() > 0;
            }
            else
            {
                throw new BadHttpRequestException($"Criteria with {criteriaId} is not existed!");
            }
        }

        public bool CreateNewQuestionGrade(Guid criteriaId, CreateNewCriteriaQuestionRequest newCriteriaQuestionRequest)
        {
            Criterion criteria = _unitOfWork.Criteria.GetById(criteriaId);
            if (criteria is not null)
            {
                newCriteriaQuestionRequest.Priority = newCriteriaQuestionRequest.Priority.ToUpper();
                Question newQuestion = _mapper.Map<Question>(newCriteriaQuestionRequest);
                newQuestion.CriteriaId = criteriaId;

                _unitOfWork.Question.Insert(newQuestion);

                return _unitOfWork.Save() > 0;
            }
            else
            {
                throw new BadHttpRequestException($"Criteria with {criteriaId} is not existed!");
            }
        }
    }
}
