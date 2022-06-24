using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository.Interfaces
{
    public interface ITeamRepository : IGenericRepository<Team>
    {
        Team GetTeamWithProject(Guid teamId);
    }
}
