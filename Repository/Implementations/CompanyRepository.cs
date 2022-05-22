using Models.Models;
using Repository.Interfaces;


namespace Repository.Implementations
{
    public class CompanyRepository : GenericRepository<Company>, ICompanyRepository
    {
        public CompanyRepository(CAPSTONEONGOINGContext context) : base(context)
        {
        }
    }
}
