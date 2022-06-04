using Models.Dtos;

namespace CapstoneOnGoing.Services.Interfaces
{
	public interface ISemesterService
	{
		CreateNewSemesterDTO CreateNewSemester(CreateNewSemesterDTO newSemester);
	}
}
