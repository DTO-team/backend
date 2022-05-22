using Models.Models;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implementations
{
    public class SemesterCriterionRepository : GenericRepository<SemesterCriterion>, ISemesterCriterionRepository
    {
        public SemesterCriterionRepository(CAPSTONEONGOINGContext context) : base(context)
        {
        }
    }
}
