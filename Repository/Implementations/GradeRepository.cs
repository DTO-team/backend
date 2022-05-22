using Models.Models;
using Repository.Interfaces;


namespace Repository.Implementations
{
    public class GradeRepository : GenericRepository<Grade>, IGradeRepository
    {
        public GradeRepository(CAPSTONEONGOINGContext context) : base(context)
        {
        }
    }
}
