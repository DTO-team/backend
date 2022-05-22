using Models.Models;
using Repository.Interfaces;


namespace Repository.Implementations
{
    public class ReviewGradeRepository : GenericRepository<ReviewGrade>, IReviewGradeRepository
    {
        public ReviewGradeRepository(CAPSTONEONGOINGContext context) : base(context)
        {
        }
    }
}
