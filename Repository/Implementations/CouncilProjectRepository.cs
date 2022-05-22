using Models.Models;
using Repository.Interfaces;

namespace Repository.Implementations
{
    public class CouncilProjectRepository : GenericRepository<CouncilProject>, ICouncilProjectRepository
    {
        public CouncilProjectRepository(CAPSTONEONGOINGContext context) : base(context)
        {
        }
    }
}
