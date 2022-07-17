using Models.Request;

namespace CapstoneOnGoing.Services.Interfaces
{
	public interface ICouncilService
	{
		void CreateCouncil(CreateCouncilRequest createCouncilRequest);
	}
}