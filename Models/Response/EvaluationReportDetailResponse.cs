using System;

namespace Models.Response
{
    public class EvaluationReportDetailResponse
    {
        public Guid Id { get; set; }
        public Guid EvaluationReportId { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
