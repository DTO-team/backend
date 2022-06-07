using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        Role GetRoleByName(string name);
        Role GetRoleById(int id);
    }
}
