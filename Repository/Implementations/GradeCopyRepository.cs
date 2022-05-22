using Models.Models;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implementations
{
    public class GradeCopyRepository : GenericRepository<GradeCopy>, IGradeCopyRepository
    {
        public GradeCopyRepository(CAPSTONEONGOINGContext context) : base(context)
        {
        }
    }
}
