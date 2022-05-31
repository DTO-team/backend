using Models.Models;
using Repository.Interfaces;


namespace Repository.Implementations
{
    public class TeamStudentRepository : GenericRepository<TeamStudent>, ITeamStudentRepository
    {
        public TeamStudentRepository(CAPSTONEONGOINGContext context) : base(context)
        {
        }
    }
}
