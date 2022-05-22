using Models.Models;
using Repository.Interfaces;


namespace Repository.Implementations
{
    public class EvaluationSessionRepository : GenericRepository<EvaluationSession>, IEvaluationSessionRepository
    {
        public EvaluationSessionRepository(CAPSTONEONGOINGContext context) : base(context)
        {
        }
    }
}
