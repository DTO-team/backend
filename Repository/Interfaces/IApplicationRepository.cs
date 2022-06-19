using Models.Models;
using System;
using System.Collections.Generic;
using Models.Request;

namespace Repository.Interfaces
{
    public interface IApplicationRepository : IGenericRepository<Application>
    {
        IEnumerable<Application> GetAllApplicationsWithTeamTopicProject();
        Application GetApplicationWithTeamTopicProject(Guid id);

        Application GetApplicationWithTeamTopicProjectByTeamIdAndTopicId(
            CreateNewApplicationRequest newApplicationRequest);
    }
}
