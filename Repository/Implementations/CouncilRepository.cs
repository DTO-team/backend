using Models.Models;
using Repository.Interfaces;


namespace Repository.Implementations
{
    public class CouncilRepository : GenericRepository<Council>, ICouncilRepository
    {
        public CouncilRepository(CAPSTONEONGOINGContext context) : base(context)
        {
        }
    }
}
