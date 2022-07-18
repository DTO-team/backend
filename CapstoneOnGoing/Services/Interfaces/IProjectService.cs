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
        GetProjectDetailDTO GetProjectDetailById(Guid projectId, GetSemesterDTO semester);
        IEnumerable<GetProjectDetailDTO> GetAllCouncilProject(Guid councilId, GetSemesterDTO semester);

    }
}
