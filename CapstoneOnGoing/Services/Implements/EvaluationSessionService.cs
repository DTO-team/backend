using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CapstoneOnGoing.Enums;
using CapstoneOnGoing.Logger;
using CapstoneOnGoing.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Models.Models;
using Models.Request;
using Models.Response;
using Repository.Interfaces;

namespace CapstoneOnGoing.Services.Implements
{
	public class EvaluationSessionService : IEvaluationSessionService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ILoggerManager _logger;
		private readonly ICouncilService _councilService;
		private readonly IMapper _mapper;
		private readonly ILecturerService _lecturerService;

		public EvaluationSessionService(IUnitOfWork unitOfWork, ILoggerManager logger,ICouncilService councilService, IMapper mapper, ILecturerService lecturerService)
		{
			_unitOfWork = unitOfWork;
			_logger = logger;
			_councilService = councilService;
			_mapper = mapper;
			_lecturerService = lecturerService;
		}

		public bool UpdateEvaluationSessionStatus(UpdateEvaluationSessionRequest updateEvaluationSessionRequest)
		{
			bool isSuccessful = false;
			if (updateEvaluationSessionRequest != null)
			{
				EvaluationSession updatedEvaluationSession = _unitOfWork.EvaluationSession.GetById(updateEvaluationSessionRequest.Id);
				if (updatedEvaluationSession == null) throw new BadHttpRequestException("No Evaluation session found");
				switch (updateEvaluationSessionRequest.Status)
				{
					case (int)EvaluationSessionStatus.ONGOING:
						updatedEvaluationSession.Status = updateEvaluationSessionRequest.Status;
						_unitOfWork.EvaluationSession.Update(updatedEvaluationSession);
						_unitOfWork.Save();
						//Create council when evaluation session status is OnGoing
						_councilService.CreateCouncil(updateEvaluationSessionRequest.CreateCouncilRequest);
						isSuccessful =  _unitOfWork.Save() > 0;
						break;
					case (int)EvaluationSessionStatus.ENDED:
						updatedEvaluationSession.Status = updateEvaluationSessionRequest.Status;
						_unitOfWork.EvaluationSession.Update(updatedEvaluationSession);
						isSuccessful = _unitOfWork.Save() > 0;
						break;
				}
			}
			return isSuccessful;
		}

		public IEnumerable<GetEvaluationSessionResponse> GetAllEvaluationSession(Guid semesterGuid)
		{
			List<GetEvaluationSessionResponse> getEvaluationSessionResponses = new List<GetEvaluationSessionResponse>();
			IEnumerable<EvaluationSession> evaluationSessions = _unitOfWork.EvaluationSession.Get(x => x.SemesterId == semesterGuid);
			Array.ForEach(evaluationSessions.ToArray(), evaluationSession =>
			{
				GetEvaluationSessionResponse getEvaluationSessionResponse =
					GetEvaluationSessionById(evaluationSession.Id, semesterGuid);
				getEvaluationSessionResponses.Add(getEvaluationSessionResponse);
			});
			return getEvaluationSessionResponses.OrderBy(x => x.DeadLine);
		}

		public GetEvaluationSessionResponse GetEvaluationSessionById(Guid evaluationId, Guid semesterId)
		{
			EvaluationSession evaluationSession = _unitOfWork.EvaluationSession
				.Get(x => x.Id == evaluationId && x.SemesterId == semesterId, null, "Semester,EvaluationSessionCriteria,Councils").FirstOrDefault();
			if (evaluationSession == null)
			{
				throw new BadHttpRequestException("No evaluation session found");
			}

			GetEvaluationSessionResponse evaluationSessionResponse =
				_mapper.Map<GetEvaluationSessionResponse>(evaluationSession);
			if (evaluationSession.EvaluationSessionCriteria.Any())
			{
				evaluationSessionResponse.SemesterCriterias = new List<GetSemesterCriteriaResponse>();
				Array.ForEach(evaluationSession.EvaluationSessionCriteria.ToArray(), evaluationSessionCriteria =>
				{
					IEnumerable<SemesterCriterion> semesterCriterion =
						_unitOfWork.SememesterCriterion.Get(x => x.Id == evaluationSessionCriteria.CriteriaId);
					IEnumerable<GetSemesterCriteriaResponse> semesterCriteriaResponses =
						_mapper.Map<IEnumerable<GetSemesterCriteriaResponse>>(semesterCriterion);
					evaluationSessionResponse.SemesterCriterias.AddRange(semesterCriteriaResponses);
				});
			}

			if (evaluationSession.Councils.Any())
			{
				evaluationSessionResponse.LecturerInCouncils = new List<GetLecturerInCouncil>();
				Array.ForEach(evaluationSession.Councils.ToArray(), council =>
				{
					GetLecturerInCouncil getLecturerInCouncil = new GetLecturerInCouncil();
					getLecturerInCouncil.Lecturers = new List<GetLecturerResponse>();
					IEnumerable<Council> councils = _unitOfWork.Councils.Get(x => x.Id == council.Id,null, "CouncilLecturers");
					Array.ForEach(councils.ToArray(), council =>
					{
						Array.ForEach(council.CouncilLecturers.ToArray(), councilLecturer =>
						{
							User lecturer = _lecturerService.GetLecturerById(councilLecturer.LecturerId);
							GetLecturerResponse lecturerResponse = _mapper.Map<GetLecturerResponse>(lecturer);
							getLecturerInCouncil .Lecturers.Add(lecturerResponse);
						});
					});
					evaluationSessionResponse.LecturerInCouncils.Add(getLecturerInCouncil);
				});
			}
			return evaluationSessionResponse;
		}
	}
}