using Models.Models;
using System;
using System.Collections.Generic;

namespace Repository.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
	    IEnumerable<string> GetFullNameById(IEnumerable<Guid> ids);
    }
}
