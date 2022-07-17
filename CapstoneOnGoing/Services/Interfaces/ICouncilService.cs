using Models.Request;

namespace CapstoneOnGoing.Services.Interfaces
{
	public interface ICouncilService
	{
		bool CreateCouncil(CreateCouncilRequest createCouncilRequest);
	}
}