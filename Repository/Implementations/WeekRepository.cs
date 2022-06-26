using Models.Models;
using Repository.Interfaces;

namespace Repository.Implementations
{
	public class WeekRepository : GenericRepository<Week>, IWeekRepository
	{
		public WeekRepository(CAPSTONEONGOINGContext context) : base(context)
		{
		}
	}
}
