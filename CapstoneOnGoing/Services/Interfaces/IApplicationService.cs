using System;
using System.Collections.Generic;
using Models.Dtos;
using Models.Request;

namespace CapstoneOnGoing.Services.Interfaces
{
    public interface IApplicationService
    {
        GetApplicationDTO CreateNewApplication(CreateNewApplicationRequest newApplicationRequest);
        GetApplicationDTO GetApplicationById(Guid id);
        IEnumerable<GetApplicationDTO> GetAllApplication();
        bool UpdateApplicationStatusById(Guid id, UpdateApplicationStatusRequest request);

    }
}
