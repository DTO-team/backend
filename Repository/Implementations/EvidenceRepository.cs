using Models.Models;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repository.Implementations
{
    public class EvidenceRepository : GenericRepository<Evidence>, IEvidenceRepository
    {
        public EvidenceRepository(CAPSTONEONGOINGContext context) : base(context)
        {
        }
    }
}
