using System;
using System.Collections.Generic;
using Models.Models;

namespace Models.Response
{
    public class GetAllEvaluationReportResponse
    {
        public GetEvaluationSessionResponse EvaluationSession { get; set; }
        public IEnumerable<EvaluationReportDetailResponse> EvaluationReportsDetail { get; set; }
        public GetTeamDetailResponse TeamDetail { get; set; }
    }
}
