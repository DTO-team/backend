using Models.Models;
using Repository.Interfaces;


namespace Repository.Implementations
{
    public class LecturerRepository : GenericRepository<Lecturer>, ILecturerRepository
    {
        public LecturerRepository(CAPSTONEONGOINGContext context) : base(context)
        {
        }
    }
}
