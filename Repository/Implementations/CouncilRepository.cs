using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using Repository.Interfaces;


namespace Repository.Implementations
{
    public class CouncilRepository : GenericRepository<Council>, ICouncilRepository
    {
        public CouncilRepository(CAPSTONEONGOINGContext context) : base(context)
        {
        }

        public Council GetCouncilWithProjectAndTeamById(Guid councilId)
        {
            Council council = dbSet
                .Include(council => council.CouncilProjects)
                .ThenInclude(councilProject => councilProject.Project)
                .Include(council => council.CouncilLecturers)
                .Where(coucil => coucil.Id.Equals(councilId)).FirstOrDefault();
            return council;
        }
    }
}
