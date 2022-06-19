using Models.Models;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Models.Request;

namespace Repository.Implementations
{
    public class ApplicationRepository : GenericRepository<Application>, IApplicationRepository
    {
        public ApplicationRepository(CAPSTONEONGOINGContext context) : base(context)
        {
        }

        public IEnumerable<Application> GetAllApplicationsWithTeamTopicProject()
        {
            IEnumerable<Application> applications = dbSet.
                Include(x => x.Team).
                Include(x => x.Topic).
                Include(x => x.Project).
                AsNoTracking();
            return applications;
        }

        public Application GetApplicationWithTeamTopicProject(Guid Id)
        {
            Application result = dbSet
                 .Include(x => x.Team)
                 .Include(x => x.Project)
                 .Include(x => x.Topic)
                 .AsNoTracking()
                 .FirstOrDefault(application => application.Id == Id);
            return result;
        }

        public Application GetApplicationWithTeamTopicProjectByTeamIdAndTopicId(CreateNewApplicationRequest newApplicationRequest)
        {
            Guid requestTeamId = newApplicationRequest.TeamId;
            Guid requestTopicId = newApplicationRequest.TopicId;
            Application result = dbSet
                 .Include(x => x.Team)
                 .Include(x => x.Project)
                 .Include(x => x.Topic)
                 .AsNoTracking()
                 .FirstOrDefault(application => (application.TeamId == requestTeamId && application.TopicId == requestTopicId));
            return result;
        }
    }
}
