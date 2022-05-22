using Models.Models;
using Repository.Interfaces;


namespace Repository.Implementations
{
    public class SemesterRepository : GenericRepository<Semester>, ISemesterRepository
    {
        public SemesterRepository(CAPSTONEONGOINGContext context) : base(context)
        {
        }
    }
}
