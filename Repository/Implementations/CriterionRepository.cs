using Models.Models;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implementations
{
    public class CriterionRepository : GenericRepository<Criterion>, ICriterionRepository
    {
        public CriterionRepository(CAPSTONEONGOINGContext context) : base(context)
        {
        }
    }
}
