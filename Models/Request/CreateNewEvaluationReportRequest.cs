using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models.Request
{

    public class EvaluationReportDetailRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Url { get; set; }
    }

    public class CreateNewEvaluationReportRequest
    {
        public Guid TeamId { get; set; }
        public virtual ICollection<EvaluationReportDetailRequest> EvaluationReportDetailsRequest { get; set; }

    }
}
