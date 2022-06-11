using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CapstoneOnGoing.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Models.Models;
using Models.Request;
using Repository.Interfaces;

namespace CapstoneOnGoing.Services.Implements
{
	public class TopicService : ITopicService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IMapper _mapper;
    
		public TopicService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}
		public bool ImportTopics(IEnumerable<ImportTopicsRequest> importTopicsRequest)
		{
			bool isSuccessful = true;
			Semester currentSemester = _unitOfWork.Semester.Get(x => x.Status == 1).FirstOrDefault();
			if (currentSemester != null)
			{
				foreach (ImportTopicsRequest importTopic in importTopicsRequest)
				{
					//Get lecture
					User lecturer = _unitOfWork.User.Get(x => (importTopic.LecturerEmail.Equals(x.Email) && x.RoleId == 2)).FirstOrDefault();
					User company = null;
					if (!string.IsNullOrEmpty(importTopic.CompanyEmail) &&
					    !string.IsNullOrWhiteSpace(importTopic.CompanyEmail))
					{
						company = _unitOfWork.User.Get(x => (x.RoleId == 4 && importTopic.CompanyEmail.Equals(x.Email)), null, "Company").FirstOrDefault();
						if (company == null)
						{
							throw new BadHttpRequestException(
								$"Company with email: {importTopic.CompanyEmail} does not exist");
						}
					}//end If the topic related to company
					if (lecturer != null)
					{
						Topic importedTopic = _mapper.Map<Topic>(importTopic);
						importedTopic.CompanyId = company?.Id;
						importedTopic.SemesterId = currentSemester.Id;
						importedTopic.TopicLecturers.Add(new TopicLecturer()
						{
							LecturerId = lecturer.Id,
							TopicId = importedTopic.Id
						});
						_unitOfWork.Topic.Insert(importedTopic);
					}
					else
					{
						throw new BadHttpRequestException($"Lecturer with {importTopic.LecturerEmail} does not exist");
					}
				}
			}
			else
			{
				isSuccessful = false;
			}

			if (isSuccessful)
			{
				_unitOfWork.Save();
			}
			return isSuccessful;
		}
	}
}
