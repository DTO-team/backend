using Models.Models;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implementations
{
    public class SemesterRepository : GenericRepository<Semester>, ISemesterRepository
    {
        public SemesterRepository(CAPSTONEONGOINGContext context) : base(context)
        {
        }
    }
}
