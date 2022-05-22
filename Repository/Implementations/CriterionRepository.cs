using Models.Models;
using Repository.Interfaces;


namespace Repository.Implementations
{
    public class CriterionRepository : GenericRepository<Criterion>, ICriterionRepository
    {
        public CriterionRepository(CAPSTONEONGOINGContext context) : base(context)
        {
        }
    }
}
