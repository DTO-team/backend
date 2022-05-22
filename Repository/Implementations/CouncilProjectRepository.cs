using Models.Models;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implementations
{
    public class CouncilProjectRepository : GenericRepository<CouncilProject>, ICouncilProjectRepository
    {
        public CouncilProjectRepository(CAPSTONEONGOINGContext context) : base(context)
        {
        }
    }
}
