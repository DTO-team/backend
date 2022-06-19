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

        public IEnumerable<Application> GetAllApplicationsWithTeamTopicProject(string searchString, int page, int limit, out int totalRecords)
        {
	        if (string.IsNullOrEmpty(searchString) || string.IsNullOrWhiteSpace(searchString))
	        {
		        IEnumerable<Application> result = dbSet
			        .Include(x => x.Team)
			        .Include(x => x.Topic)
			        .Include(x => x.Project)
			        .AsNoTracking()
			        .ToList();
		        totalRecords = result.Count();
		        var offset = (page - 1) * limit;
		        IEnumerable<Application> applications = result.Skip(offset).Take(limit);
		        return applications;
            }
	        else
	        {
		        IEnumerable<Application> result = dbSet
			        .Include(x => x.Team)
			        .Include(x => x.Topic)
			        .Include(x => x.Project)
			        .AsNoTracking()
			        .Where(x => x.Team.Name.Contains(searchString));
		        totalRecords = result.Count();
		        var offset = (page - 1) * limit;
		        IEnumerable<Application> applications = result.Skip(offset).Take(limit);
		        return applications;
            }
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
