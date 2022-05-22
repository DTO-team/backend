using Models.Models;
using Repository.Interfaces;


namespace Repository.Implementations
{
    public class ReviewQuestionRepository : GenericRepository<ReviewQuestion>, IReviewQuestionRepository
    {
        public ReviewQuestionRepository(CAPSTONEONGOINGContext context) : base(context)
        {
        }
    }
}
