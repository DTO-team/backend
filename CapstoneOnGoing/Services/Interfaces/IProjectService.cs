using System;
using System.Collections.Generic;
using CapstoneOnGoing.Filter;
using Models.Dtos;
using Models.Response;

namespace CapstoneOnGoing.Services.Interfaces
{
    public interface IProjectService
    {
        IEnumerable<GetAllProjectsDetailDTO> GetAllProjectResponse(PaginationFilter validFilter, out int totalRecords);
        GetProjectDetailDTO GetProjectDetailById(Guid projectId);
        IEnumerable<GetAllProjectsDetailDTO> GetAllCouncilProject(Guid councilId);

    }
}
