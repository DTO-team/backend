using Models.Models;
using Repository.Interfaces;


namespace Repository.Implementations
{
    public class ApplicationStatusRepository : GenericRepository<ApplicationStatus>, IApplicationStatusRepository
    {
        public ApplicationStatusRepository(CAPSTONEONGOINGContext context) : base(context)
        {
        }
    }
}
