using Models.Models;
using Repository.Interfaces;

namespace Repository.Implementations
{
	public class FeedbackRepository : GenericRepository<Feedback>,IFeedbackRepository
	{
		public FeedbackRepository(CAPSTONEONGOINGContext context) : base(context)
		{
		}
	}
}