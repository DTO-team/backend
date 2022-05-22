using Models.Models;
using Repository.Interfaces;

namespace Repository.Implementations
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(CAPSTONEONGOINGContext context) : base(context)
        {
        }
    }
}
