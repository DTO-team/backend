using System;
using System.Collections.Generic;
using Models.Dtos;

namespace CapstoneOnGoing.Services.Interfaces
{
    public interface ICriterionService
    {
        IEnumerable<CriteriaDTO> GetAllCriteria();
        CriteriaDTO GetCriteriaById(Guid criteriaId);
    }
}
