using System;
using AutoMapper;
using CapstoneOnGoing.Logger;
using CapstoneOnGoing.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Models.Dtos;
using Models.Models;
using Models.Request;
using Repository.Interfaces;

namespace CapstoneOnGoing.Services.Implements
{
	public class ReportService : IReportService
	{
		private readonly ILoggerManager _logger;
		private readonly IMapper _mapper;
		private readonly IUnitOfWork _unitOfWork;
		public ReportService(ILoggerManager logger, IMapper mapper, IUnitOfWork unitOfWork)
		{
			_logger = logger;
			_mapper = mapper;
			_unitOfWork = unitOfWork;
		}
		public bool CreateWeeklyReport(Guid teamId, string studentEmail, CreateWeeklyReportDTO createWeeklyReportDTO)
		{
			//check team is exist
			Team team = _unitOfWork.Team.GetById(teamId);
			
			if (team == null)
			{
				throw new BadHttpRequestException("Team does not exist");
			}
			return true;
		}
	}
}
