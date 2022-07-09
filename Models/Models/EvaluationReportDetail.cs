using System;
using System.Collections.Generic;

#nullable disable

namespace Models.Models
{
    public partial class EvaluationReportDetail
    {
        public Guid Id { get; set; }
        public Guid EvaluationReportId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }

        public virtual EvaluationReport EvaluationReport { get; set; }
    }
}
