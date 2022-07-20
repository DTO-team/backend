using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Models;
using Repository.Interfaces;

namespace Repository.Implementations
{
    public class EvaluationReportRepository : GenericRepository<EvaluationReport>, IEvaluationReportRepository
    {
        public EvaluationReportRepository(CAPSTONEONGOINGContext context) : base(context)
        {
        }
    }
}
