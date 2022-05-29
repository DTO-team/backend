using Models.Models;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Repository.Implementations
{
    public class ReportRepository : GenericRepository<Report>, IReportRepository
    {
        public ReportRepository(CAPSTONEONGOINGContext context) : base(context)
        {
        }
    }
}
