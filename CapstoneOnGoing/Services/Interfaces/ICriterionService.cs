using System;
using System.Collections.Generic;
using Models.Dtos;
using Models.Request;

namespace CapstoneOnGoing.Services.Interfaces
{
    public interface ICriterionService
    {
        IEnumerable<CriteriaDTO> GetAllCriteria();
        CriteriaDTO GetCriteriaById(Guid criteriaId);
        CriteriaDTO GetCriteriaByCode(string criteriaCode);
        bool CreateNewCriteria(CreateCriteriaRequest newCriteriaRequest);
        bool UpdateCriteria(Guid criteriaId,UpdateCriteriaRequest updateCriteriaRequest);
        bool DeleteCriteria(Guid criteriaId);
        bool CreateNewCriteriaGrade(Guid criteriaId,CreateNewCriteriaGradeRequest newCriteriaGradeRequest);
        bool CreateNewQuestionGrade(Guid criteriaId, CreateNewCriteriaQuestionRequest newCriteriaQuestionRequest);
    }
}
