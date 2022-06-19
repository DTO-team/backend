using System;
using System.Collections.Generic;
using CapstoneOnGoing.Filter;
using Models.Dtos;
using Models.Request;

namespace CapstoneOnGoing.Services.Interfaces
{
    public interface IApplicationService
    {
        GetApplicationDTO CreateNewApplication(CreateNewApplicationRequest newApplicationRequest);
        GetApplicationDTO GetApplicationById(Guid id);
        IEnumerable<GetApplicationDTO> GetAllApplications(PaginationFilter validFilter,out int totalRecords);
        bool UpdateApplicationStatusById(Guid id, UpdateApplicationStatusRequest request);

    }
}
