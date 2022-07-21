using System;
using Models.Models;

namespace Models.Response
{
    public class GetEvaluationReportDetailResponse
    {
        public GetEvaluationSessionResponse EvaluationSession { get; set; }
        public EvaluationReportDetailResponse EvaluationReportDetail { get; set; }
        public GetTeamDetailResponse TeamDetail { get; set; }
    }
}
