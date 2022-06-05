using Models.Dtos;
using Models.Models;
using System;
using System.Collections.Generic;

namespace CapstoneOnGoing.Services.Interfaces
{
	public interface ISemesterService
	{
		Semester GetSemesterById(Guid id);
		Semester CreateNewSemester(Semester newSemester);

		IEnumerable<Semester> GetAllSemesters(int page, int limit);

		bool UpdateSemester(Semester updatedSemester);
	}
}
