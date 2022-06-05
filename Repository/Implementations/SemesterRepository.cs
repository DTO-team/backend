using Models.Models;
using Repository.Interfaces;
using System.Linq;

namespace Repository.Implementations
{
    public class SemesterRepository : GenericRepository<Semester>, ISemesterRepository
    {
        public SemesterRepository(CAPSTONEONGOINGContext context) : base(context)
        {
        }

		public Semester GetSemesterByYearAndSession(int year, string season)
		{
			return context.Semesters.FirstOrDefault(semester => (semester.Year == year && semester.Season.Equals(season)));
		}
	}
}
