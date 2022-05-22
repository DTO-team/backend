using Models.Models;
using Repository.Interfaces;


namespace Repository.Implementations
{
    public class EvaluationSessionCriterionRepository : GenericRepository<EvaluationSessionCriterion>, IEvaluationSessionCriterionRepository
    {
        public EvaluationSessionCriterionRepository(CAPSTONEONGOINGContext context) : base(context)
        {
        }
    }
}
