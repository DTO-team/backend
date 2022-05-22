using Models.Models;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implementations
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        public RoleRepository(CAPSTONEONGOINGContext context) : base(context)
        {
        }
    }
}
