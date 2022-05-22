using Models.Models;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implementations
{
    public class CouncilLecturerRepository : GenericRepository<Council>, ICouncilLecturerRepository
    {
        public CouncilLecturerRepository(CAPSTONEONGOINGContext context) : base(context)
        {
        }
    }
}
