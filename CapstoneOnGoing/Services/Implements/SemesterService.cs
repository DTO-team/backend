using CapstoneOnGoing.Services.Interfaces;
using Models.Dtos;
using Models.Models;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using CapstoneOnGoing.Enums;
using CapstoneOnGoing.Helper;
using Microsoft.AspNetCore.Http;
using Models.Request;
using Models.Response;

namespace CapstoneOnGoing.Services.Implements
{
	public class SemesterService : ISemesterService
	{
		private readonly IUnitOfWork _unitOfWork;
		private const int GetSunday = 6;
		private const int GetFriday = 4;
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
						CreateEvaluationSession(semesterDto);
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

		public Week GetCurrentWeek(Guid semesterId, long currentDateTime)
		{
			Semester currentSemester = _unitOfWork.Semester.Get(x => x.Id == semesterId, null, "Weeks").FirstOrDefault();
			Week currentWeek = null;
			if (currentSemester == null || !currentSemester.Weeks.Any())
			{
				throw new BadHttpRequestException("Current Semester is not exist");
			}
			else
			{
				currentWeek = currentSemester.Weeks.FirstOrDefault(x => x.FromDate >= currentDateTime && currentDateTime <= x.ToDate);
				return currentWeek;
			}
		}

		public IEnumerable<Week> GetWeeksOfSemester(Guid semesterId)
		{
			if (!semesterId.Equals(Guid.Empty) || semesterId != null)
			{
				IEnumerable<Week> weeks = _unitOfWork.Week.Get(x => x.SemesterId == semesterId);
				return weeks;
			}

			return null;
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
					Deadline = DateTimeHelper.ConvertDateTimeToLong(startDateOfSemester.AddDays(GetFriday).AddHours(12)),
					SemesterId = semester.Id,
				};
				weeksOfSemester.Add(newWeek);
				startDateOfSemester = previousMonday.AddDays(GetNextMonday);
			}
			_unitOfWork.Week.InsertRange(weeksOfSemester);
		}

		private void CreateEvaluationSession(UpdateSemesterDTO updatedSemester)
		{
			if (updatedSemester.CreateEvaluationSessionRequests != null)
			{
				ICollection<EvaluationSession> evaluationSessions = new List<EvaluationSession>();
				ICollection<EvaluationSessionCriterion> evaluationSessionCriteriaList =
					new List<EvaluationSessionCriterion>();
				ICollection<SemesterCriterion> semesterCriteriaList = new List<SemesterCriterion>();
				ICollection<GradeCopy> gradeCopyList = new List<GradeCopy>();
				ICollection<QuestionCopy> questionCopyList = new List<QuestionCopy>();
				Array.ForEach(updatedSemester.CreateEvaluationSessionRequests.ToArray(), createEvaluationSessionRequest =>
				{
					EvaluationSession evaluationSession = new EvaluationSession()
					{
						Id = Guid.NewGuid(),
						SemesterId = updatedSemester.Id,
						Round = createEvaluationSessionRequest.Round,
						IsFinal = createEvaluationSessionRequest.IsFinal,
						Status = createEvaluationSessionRequest.Status,
						Deadline = createEvaluationSessionRequest.Deadline,
					};
					Array.ForEach(createEvaluationSessionRequest.Criterias.ToArray(), criteria =>
					{
						IEnumerable<Grade> grades = _unitOfWork.Grade.Get(x => x.CriteriaId == criteria.Id);
						IEnumerable<Question> questions = _unitOfWork.Question.Get(x => x.CriteriaId == criteria.Id);
						if (!grades.Any() || !questions.Any())
						{
							throw new BadHttpRequestException($"No questions or grades for {criteria.Name} criteria");
						}
						Array.ForEach(grades.ToArray(), grade =>
						{
							
						});
						SemesterCriterion semesterCriterion = new SemesterCriterion()
						{
							Id = criteria.Id,
							Name = criteria.Name,
							Code = criteria.Code,
							SemesterId = updatedSemester.Id,
							Evaluation = criteria.Evaluation
						};
						EvaluationSessionCriterion evaluationSessionCriterion = new EvaluationSessionCriterion()
						{
							Id = Guid.NewGuid(),
							CriteriaId = criteria.Id,
							EvaluationSessionId = evaluationSession.Id
						};
						semesterCriteriaList.Add(semesterCriterion);
						evaluationSessionCriteriaList.Add(evaluationSessionCriterion);
					});
					evaluationSessions.Add(evaluationSession);
				});
				_unitOfWork.SememesterCriterion.InsertRange(semesterCriteriaList);
				_unitOfWork.EvaluationSessionCriterion.InsertRange(evaluationSessionCriteriaList);
				_unitOfWork.EvaluationSession.InsertRange(evaluationSessions);
			}
		}
	}
}
