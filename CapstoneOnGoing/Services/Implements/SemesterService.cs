using CapstoneOnGoing.Services.Interfaces;
using Models.Dtos;
using Models.Models;
using Repository.Interfaces;

namespace CapstoneOnGoing.Services.Implements
{
	public class SemesterService : ISemesterService
	{
		private readonly IUnitOfWork _unitOfWork;
		public SemesterService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public bool CreateNewSemester(Semester newSemester)
		{
			//Check duplicated
			Semester semester = _unitOfWork.Semester.GetSemesterByYearAndSession(newSemester.Year,newSemester.Season);
			if(semester != null){
				return false;
			}
			else{
				//set status of new semester is Preparing
				newSemester.Status = 1;
				_unitOfWork.Semester.Insert(newSemester);
				return true;
			}
		}
	}
}
