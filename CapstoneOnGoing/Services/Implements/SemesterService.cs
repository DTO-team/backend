using CapstoneOnGoing.Services.Interfaces;
using Models.Dtos;
using Models.Models;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using CapstoneOnGoing.Enums;
using CapstoneOnGoing.Helper;
using Microsoft.AspNetCore.Http;

namespace CapstoneOnGoing.Services.Implements
{
	public class SemesterService : ISemesterService
	{
		private readonly IUnitOfWork _unitOfWork;
		private const int GetSunday = 6;
		private const int GetNextMonday = 7;
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
				newSemester.Status = (int)SemesterStatus.Preparing;
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
				case (int)SemesterStatus.Preparing:
					if (semesterDto.Status == (int)SemesterStatus.Ongoing)
					{
						updatedSemester.Status = semesterDto.Status;
						//Generate week based on start day of semester
						GenerateWeeksForSemester(updatedSemester, semesterDto);
						_unitOfWork.Semester.Update(updatedSemester);
						_unitOfWork.Save();
						isSuccessful = true;
					}
					break;
				case (int)SemesterStatus.Ongoing:
					if (semesterDto.Status == (int)SemesterStatus.Ended)
					{
						updatedSemester.Status = semesterDto.Status;
						_unitOfWork.Semester.Update(updatedSemester);
						_unitOfWork.Save();
						isSuccessful = true;
					}
					break;
			}
			return isSuccessful;
		}

		private void GenerateWeeksForSemester(Semester semester, UpdateSemesterDTO semesterDto)
		{
			DateTime startDateOfSemester = DateTimeHelper.ConvertLongToDateTime(semesterDto.StartDayOfSemester);
			if (startDateOfSemester.DayOfWeek != DayOfWeek.Monday)
			{
				throw new BadHttpRequestException("Start date of semester must be Monday");
			}

			int numberWeeksOfSemester = 1;
			DateTime previousMonday;
			ICollection<Week> weeksOfSemester = new List<Week>();
			while (weeksOfSemester.Count < 15)
			{
				previousMonday = startDateOfSemester;
				Week newWeek = new Week()
				{
					Number = numberWeeksOfSemester++,
					FromDate = DateTimeHelper.ConvertDateTimeToLong(startDateOfSemester),
					ToDate = DateTimeHelper.ConvertDateTimeToLong(startDateOfSemester.AddDays(GetSunday)),
					SemesterId = semester.Id,
				};
				weeksOfSemester.Add(newWeek);
				startDateOfSemester = previousMonday.AddDays(GetNextMonday);
			}
			_unitOfWork.Week.InsertRange(weeksOfSemester);
		}
	}
}
