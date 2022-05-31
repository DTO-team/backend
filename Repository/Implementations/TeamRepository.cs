using Models.Models;
using Repository.Interfaces;
namespace Repository.Implementations
{
    public class TeamRepository : GenericRepository<Team>, ITeamRepository
    {
        public TeamRepository(CAPSTONEONGOINGContext context) : base(context)
        {
        }
    }
}
