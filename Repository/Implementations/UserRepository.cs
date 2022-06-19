using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using Repository.Interfaces;

namespace Repository.Implementations
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(CAPSTONEONGOINGContext context) : base(context)
        {
        }

        public IEnumerable<string> GetFullNameById(IEnumerable<Guid> ids)
        {
	        foreach (Guid id in ids)
	        {
		        User user =
			        dbSet.AsQueryable().AsNoTracking().FirstOrDefault(x => x.Id == id);
		        yield return user?.FullName;
	        }
        }

        public IEnumerable<User> GetLecturersByIds(params Guid[] ids)
        {
	        return dbSet.Include(x => x.Role).Include(x => x.Lecturer).ThenInclude(x => x.Department).Where(r => ids.Contains(r.Id));
        }
    }
}
