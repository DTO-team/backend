using Models.Models;
using Repository.Interfaces;
using System.Linq;

namespace Repository.Implementations
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(CAPSTONEONGOINGContext context) : base(context)
        {
        }

        public Role GetRoleByName(string name)
        {
            return dbSet.FirstOrDefault(x => x.Name == name);
        }
        public Role GetRoleById(int id)
        {
            return dbSet.FirstOrDefault(x => x.Id == id);
        }
    }
}
