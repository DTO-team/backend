using System;

namespace Models.Request
{
    public class UpdateEvaluationReportDetailRequest
    {
        public Guid evaluationReportDetailId { get; set; }
        public EvaluationReportDetailRequest updateNewEvaluationReportDetail { get; set; }
    }
}
