using System;
using System.Collections.Generic;
using Models.Dtos;

namespace CapstoneOnGoing.Services.Interfaces
{
    public interface ICriterionService
    {
        IEnumerable<CriterionDTO> GetAllCriterion();
        CriterionDTO GetCriterionById(Guid criterionId);
    }
}
