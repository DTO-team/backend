using CapstoneOnGoing.Enums;
using CapstoneOnGoing.Logger;
using CapstoneOnGoing.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Models.Models;
using Models.Request;
using Repository.Interfaces;

namespace CapstoneOnGoing.Services.Implements
{
	public class EvaluationSessionService : IEvaluationSessionService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly ILoggerManager _logger;
		private readonly ICouncilService _councilService;

		public EvaluationSessionService(IUnitOfWork unitOfWork, ILoggerManager logger,ICouncilService councilService)
		{
			_unitOfWork = unitOfWork;
			_logger = logger;
			_councilService = councilService;
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
	}
}