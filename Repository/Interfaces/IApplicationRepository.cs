using Models.Models;
using System;
using System.Collections.Generic;
using Models.Request;

namespace Repository.Interfaces
{
    public interface IApplicationRepository : IGenericRepository<Application>
    {
        IEnumerable<Application> GetAllApplicationsWithTeamTopicProject(string searchString, int page, int limit, out int totalRecords);
        Application GetApplicationWithTeamTopicProject(Guid id);

        Application GetApplicationWithTeamTopicProjectByTeamIdAndTopicId(
            CreateNewApplicationRequest newApplicationRequest);
    }
}
