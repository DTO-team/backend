using Models.Models;
using Repository.Interfaces;


namespace Repository.Implementations
{
    public class GradeCopyRepository : GenericRepository<GradeCopy>, IGradeCopyRepository
    {
        public GradeCopyRepository(CAPSTONEONGOINGContext context) : base(context)
        {
        }
    }
}
