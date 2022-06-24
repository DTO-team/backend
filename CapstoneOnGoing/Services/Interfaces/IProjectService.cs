using System;
using System.Collections.Generic;
using Models.Response;

namespace CapstoneOnGoing.Services.Interfaces
{
    public interface IProjectService
    {
        IEnumerable<GetProjectResponse> GetAllProjectResponse();
        GetProjectResponse GetProjectDetailById(Guid projectId);
    }
}
