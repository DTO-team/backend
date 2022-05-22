using Models.Models;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implementations
{
    public class CouncilRepository : GenericRepository<Council>, ICouncilRepository
    {
        public CouncilRepository(CAPSTONEONGOINGContext context) : base(context)
        {
        }
    }
}
