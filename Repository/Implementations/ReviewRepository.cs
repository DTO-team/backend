using Models.Models;
using Repository.Interfaces;


namespace Repository.Implementations
{
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        public ReviewRepository(CAPSTONEONGOINGContext context) : base(context)
        {
        }
    }
}
