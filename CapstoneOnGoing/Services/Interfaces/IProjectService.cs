using System;
using System.Collections.Generic;
using Models.Dtos;
using Models.Response;

namespace CapstoneOnGoing.Services.Interfaces
{
    public interface IProjectService
    {
        // IEnumerable<GetProjectResponse> GetAllProjectResponse();
        GetProjectDetailDTO GetProjectDetailById(Guid projectId);
    }
}
