using Models.Models;
using Repository.Interfaces;

namespace Repository.Implementations
{
    public class UserStatusRepository : GenericRepository<UserStatus>, IUserStatusRepository
    {
        public UserStatusRepository(CAPSTONEONGOINGContext context) : base(context)
        {
        }
    }
}
