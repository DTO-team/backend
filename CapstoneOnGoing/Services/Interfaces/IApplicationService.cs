using System;
using System.Collections.Generic;
using Models.Dtos;
using Models.Request;

namespace CapstoneOnGoing.Services.Interfaces
{
    public interface IApplicationService
    {
        GetApplicationDTO GetApplicationById(Guid id);

        IEnumerable<GetApplicationDTO> GetAllApplication();
        bool UpdateApplicationStatusById(Guid id, UpdateApplicationStatusRequest request);
    }
}
