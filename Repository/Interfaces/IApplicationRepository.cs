using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IApplicationRepository : IGenericRepository<Application>
    {
        IEnumerable<Application> GetAllApplicationsWithTeamTopicProject();
        Application GetApplicationWithTeamTopicProject(Guid Id);
    }
}
