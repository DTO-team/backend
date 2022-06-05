using Models.Dtos;
using Models.Models;

namespace CapstoneOnGoing.Services.Interfaces
{
	public interface ISemesterService
	{
		bool CreateNewSemester(Semester newSemester);
	}
}
