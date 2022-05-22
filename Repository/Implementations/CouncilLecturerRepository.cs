using Models.Models;
using Repository.Interfaces;


namespace Repository.Implementations
{
    public class CouncilLecturerRepository : GenericRepository<Council>, ICouncilLecturerRepository
    {
        public CouncilLecturerRepository(CAPSTONEONGOINGContext context) : base(context)
        {
        }
    }
}
