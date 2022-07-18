using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using Repository.Interfaces;


namespace Repository.Implementations
{
    public class ProjectRepository : GenericRepository<Project>, IProjectRepository
    {
        public ProjectRepository(CAPSTONEONGOINGContext context) : base(context)
        {
        }

        public IEnumerable<Project> GetAllProjectWithMentorTeamAndTeamStudents(string searchString)
        {
            if (string.IsNullOrEmpty(searchString) || string.IsNullOrWhiteSpace(searchString))
            {
                IEnumerable<Project> result = dbSet
                    .Include(project => project.Mentors)
                    .Include(project => project.Team).ThenInclude(team => team.TeamStudents)
                    .AsNoTracking()
                    .ToList();
                
                return result;
            }
            else
            {
                IEnumerable<Project> result = dbSet
                    .Include(project => project.Mentors)
                    .Include(project => project.Team).ThenInclude(team => team.TeamStudents)
                    .Where(team => team.Team.Name.Contains(searchString))
                    .AsNoTracking()
                    .ToList();
                return result;
            }
        }
    }
}
