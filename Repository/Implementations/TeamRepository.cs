using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using Repository.Interfaces;
namespace Repository.Implementations
{
    public class TeamRepository : GenericRepository<Team>, ITeamRepository
    {
        public TeamRepository(CAPSTONEONGOINGContext context) : base(context)
        {
        }

        public Team GetTeamWithProject(Guid teamId)
        {
            Team team = dbSet
                .Where(team=>team.Id.Equals(teamId))
                .Include(team => team.TeamStudents)
                .Include(team => team.Project)
                .ThenInclude(project => project.Mentors).FirstOrDefault();
            return team;
        }
    }
}
