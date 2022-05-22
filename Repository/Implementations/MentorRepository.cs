using Models.Models;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implementations
{
    public class MentorRepository : GenericRepository<Mentor>, IMentorRepository
    {
        public MentorRepository(CAPSTONEONGOINGContext context) : base(context)
        {
        }
    }
}
