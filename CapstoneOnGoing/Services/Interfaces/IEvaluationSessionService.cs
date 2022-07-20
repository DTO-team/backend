using System;
using System.Collections.Generic;
using Models.Request;
using Models.Response;

namespace CapstoneOnGoing.Services.Interfaces
{
	public interface IEvaluationSessionService
	{
		bool UpdateEvaluationSessionStatus(Guid id,UpdateEvaluationSessionRequest updateEvaluationSessionRequest);
		IEnumerable<GetEvaluationSessionResponse> GetAllEvaluationSession(Guid semesterId);
		GetEvaluationSessionResponse GetEvaluationSessionById(Guid evaluationId,Guid semesterId);
        bool CreateNewReviewOfCouncilEvaluationSession(CreateNewReviewRequest newReviewRequest);
        bool CreateNewEvaluationSessionReport(Guid evaluationSessionId, CreateNewEvaluationReportRequest newEvaluationReportRequest);
    }
}