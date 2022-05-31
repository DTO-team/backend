using Models.Models;
using Repository.Interfaces;

namespace Repository.Implementations
{
    public class ApplicationRepository : GenericRepository<Application>, IApplicationRepository
    {
        public ApplicationRepository(CAPSTONEONGOINGContext context) : base(context)
        {
        }
    }
}
