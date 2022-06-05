using CapstoneOnGoing.Services.Interfaces;
using Models.Dtos;
using Models.Models;
using Repository.Interfaces;
using System;
using System.Collections.Generic;

namespace CapstoneOnGoing.Services.Implements
{
	public class SemesterService : ISemesterService
	{
		private readonly IUnitOfWork _unitOfWork;
		public SemesterService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public Semester CreateNewSemester(Semester newSemester)
		{
			//Check duplicated
			Semester semester = _unitOfWork.Semester.GetSemesterByYearAndSession(newSemester.Year,newSemester.Season);
			if(semester != null){
				return null;
			}
			else{
				//set status of new semester is Preparing
				newSemester.Status = 1;
				_unitOfWork.Semester.Insert(newSemester);
				_unitOfWork.Save();
				return newSemester;
			}
		}

		public IEnumerable<Semester> GetAllSemesters(int page, int limit){
			IEnumerable<Semester> semesters;
			if(page == 0 || limit == 0 || page < 0 || limit < 0){
				semesters = _unitOfWork.Semester.Get(page: 1, limit: 10);
			}
			else{
				semesters = _unitOfWork.Semester.Get(page: page, limit: limit);
			}
			return semesters;
		}

		public Semester GetSemesterById(Guid id)
		{
			return _unitOfWork.Semester.GetById(id);
		}

		public bool UpdateSemester(Semester updatedSemester, UpdateSemesterDTO semesterDto)
		{
			bool isSuccessful = false;
			switch (updatedSemester.Status)
			{
				case 1:
					if (semesterDto.Status == 2)
					{
						updatedSemester.Status = semesterDto.Status;
						_unitOfWork.Semester.Update(updatedSemester);
						_unitOfWork.Save();
						isSuccessful = true;
					}
					break;
				case 2:
					if (semesterDto.Status == 3)
					{
						updatedSemester.Status = semesterDto.Status;
						_unitOfWork.Semester.Update(updatedSemester);
						_unitOfWork.Save();
						isSuccessful = true;
					}
					else if (semesterDto.Status == 1)
					{
						updatedSemester.Status = semesterDto.Status;
						_unitOfWork.Semester.Update(updatedSemester);
						_unitOfWork.Save();
						isSuccessful = true;
					}
					break;
				case 3:
					if (semesterDto.Status == 4)
					{
						updatedSemester.Status = semesterDto.Status;
						_unitOfWork.Semester.Update(updatedSemester);
						_unitOfWork.Save();
						isSuccessful = true;
					}
					else if (semesterDto.Status == 2)
					{
						updatedSemester.Status = semesterDto.Status;
						_unitOfWork.Semester.Update(updatedSemester);
						_unitOfWork.Save();
						isSuccessful = true;
					}
					break;
				case 4:
					break;
			}

			return isSuccessful;
		}
	}
}
