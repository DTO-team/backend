using Models.Models;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implementations
{
    public class TeamRepository : GenericRepository<Team>, ITeamRepository
    {
        public TeamRepository(CAPSTONEONGOINGContext context) : base(context)
        {
        }
    }
}
