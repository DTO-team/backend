using System;
using Models.Models;
using Repository.Interfaces;

namespace Repository.Implementations
{
    public class EvaluationReportDetailRepository : GenericRepository<EvaluationReportDetail>, IEvaluationReportDetailRepository
    {
        public EvaluationReportDetailRepository(CAPSTONEONGOINGContext context) : base(context)
        {
        }
    }
}
