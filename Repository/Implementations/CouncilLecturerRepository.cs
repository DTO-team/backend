using Models.Models;
using Repository.Interfaces;


namespace Repository.Implementations
{
    public class CouncilLecturerRepository : GenericRepository<CouncilLecturer>, ICouncilLecturerRepository
    {
        public CouncilLecturerRepository(CAPSTONEONGOINGContext context) : base(context)
        {
        }
    }
}
