using System;
using System.Collections.Generic;

#nullable disable

namespace Models.Models
{
    public partial class EvaluationReport
    {
        public EvaluationReport()
        {
            EvaluationReportDetails = new HashSet<EvaluationReportDetail>();
        }

        public Guid Id { get; set; }
        public Guid EvaluationSessionId { get; set; }
        public Guid TeamId { get; set; }

        public virtual EvaluationSession EvaluationSession { get; set; }
        public virtual Team Team { get; set; }
        public virtual ICollection<EvaluationReportDetail> EvaluationReportDetails { get; set; }
    }
}
