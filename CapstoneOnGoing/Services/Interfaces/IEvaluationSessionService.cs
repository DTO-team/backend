using Models.Request;

namespace CapstoneOnGoing.Services.Interfaces
{
	public interface IEvaluationSessionService
	{
		bool UpdateEvaluationSessionStatus(UpdateEvaluationSessionRequest updateEvaluationSessionRequest);
	}
}