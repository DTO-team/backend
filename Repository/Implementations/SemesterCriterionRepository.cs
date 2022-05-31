using Models.Models;
using Repository.Interfaces;


namespace Repository.Implementations
{
    public class SemesterCriterionRepository : GenericRepository<SemesterCriterion>, ISemesterCriterionRepository
    {
        public SemesterCriterionRepository(CAPSTONEONGOINGContext context) : base(context)
        {
        }
    }
}
