using Models.Models;
using Repository.Interfaces;


namespace Repository.Implementations
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(CAPSTONEONGOINGContext context) : base(context)
        {
        }
    }
}
