using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CapstoneOnGoing.Enums;
using CapstoneOnGoing.Filter;
using CapstoneOnGoing.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Models.Dtos;
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

		public Topic GetTopicById(Guid id)
		{
			return _unitOfWork.Topic.GetById(id);
		}

		public bool ImportTopics(IEnumerable<ImportTopicsRequest> importTopicsRequest)
		{
			bool isSuccessful = true;
			Semester currentSemester = _unitOfWork.Semester.Get(x => x.Status == (int)SemesterStatus.Preparing).FirstOrDefault();
			if (currentSemester != null)
			{
				foreach (ImportTopicsRequest importTopic in importTopicsRequest)
				{
					User company = null;
					if (!string.IsNullOrEmpty(importTopic.CompanyEmail) &&
						!string.IsNullOrWhiteSpace(importTopic.CompanyEmail))
					{
						company = _unitOfWork.User.Get(x => (x.RoleId == (int)RoleEnum.Company && importTopic.CompanyEmail.Equals(x.Email)), null, "Company").FirstOrDefault();
						if (company == null)
						{
							throw new BadHttpRequestException(
								$"Company with email: {importTopic.CompanyEmail} does not exist");
						}
					}//end If the topic related to company
					 //Get lectures
					Topic importedTopic = _mapper.Map<Topic>(importTopic);
					importedTopic.CompanyId = company?.Id;
					importedTopic.SemesterId = currentSemester.Id;
					Array.ForEach(importTopic.LecturerEmail.ToArray(), lecturerEmail =>
					{
						User lecturer = _unitOfWork.User.Get(x => (lecturerEmail.Equals(x.Email) && x.RoleId == (int)RoleEnum.Lecturer)).FirstOrDefault();
						if (lecturer != null)
						{
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
					});
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

		public IEnumerable<GetTopicsDTO> GetAllTopics(PaginationFilter validFilter, out int totalRecords)
		{
			//get current semester 
			Semester currentSemester = _unitOfWork.Semester.Get(x => x.Status == (int)SemesterStatus.Preparing).FirstOrDefault();
			if (currentSemester is not null)
			{
				if (string.IsNullOrEmpty(validFilter.SearchString) ||
					string.IsNullOrWhiteSpace(validFilter.SearchString))
				{
					totalRecords = _unitOfWork.Topic.Get(x => (x.SemesterId == currentSemester.Id && x.Name.Contains(validFilter.SearchString))).Count();
					IEnumerable<Topic> topics = _unitOfWork.Topic.Get(x => (x.SemesterId == currentSemester.Id && x.Name.Contains(validFilter.SearchString)), null, "TopicLecturers", validFilter.PageNumber, validFilter.PageSize);
					IEnumerable<GetTopicsDTO> getTopicsDtos = null;
					if (topics.Any())
					{
						getTopicsDtos = _mapper.Map<IEnumerable<GetTopicsDTO>>(topics);
					}
					else
					{
						totalRecords = 0;
						return null;
					}
					Array.ForEach(getTopicsDtos.ToArray(), getTopicsDto =>
					{
						IEnumerable<User> lecturers = _unitOfWork.User.GetLecturersByIds(getTopicsDto.LecturerIds.ToArray());
						getTopicsDto.LecturerDtos = _mapper.Map<IEnumerable<GetLecturerDTO>>(lecturers);
						if (getTopicsDto.CompanyId != null || getTopicsDto.CompanyId == Guid.Empty)
						{
							User companyUser = _unitOfWork.User.Get(x => x.Id == getTopicsDto.CompanyId, null, "Role").FirstOrDefault();
							getTopicsDto.CompanyDto = _mapper.Map<GetCompanyDTO>(companyUser);
						}
					});
					return getTopicsDtos;
				}
				else
				{
					totalRecords = _unitOfWork.Topic.Get(x => x.SemesterId == currentSemester.Id).Count();
					IEnumerable<Topic> topics = _unitOfWork.Topic.Get(x => x.SemesterId == currentSemester.Id, null, "TopicLecturers", validFilter.PageNumber, validFilter.PageSize);
					IEnumerable<GetTopicsDTO> getTopicsDtos = null;
					if (topics.Any())
					{
						getTopicsDtos = _mapper.Map<IEnumerable<GetTopicsDTO>>(topics);
					}
					else
					{
						totalRecords = 0;
						return null;
					}
					Array.ForEach(getTopicsDtos.ToArray(), getTopicsDto =>
					{
						IEnumerable<User> lecturers = _unitOfWork.User.GetLecturersByIds(getTopicsDto.LecturerIds.ToArray());
						getTopicsDto.LecturerDtos = _mapper.Map<IEnumerable<GetLecturerDTO>>(lecturers);
						if (getTopicsDto.CompanyId != null || getTopicsDto.CompanyId == Guid.Empty)
						{
							User companyUser = _unitOfWork.User.Get(x => x.Id == getTopicsDto.CompanyId,null,"Role").FirstOrDefault();
							getTopicsDto.CompanyDto = _mapper.Map<GetCompanyDTO>(companyUser);
						}
					});
					return getTopicsDtos;
				}
			}
			else
			{
				totalRecords = 0;
				return null;
			}
		}

		public GetTopicsDTO GetTopicDetails(Guid topicId)
		{
			GetTopicsDTO topicDto = null;
			Topic topic = _unitOfWork.Topic.Get(x => x.Id == topicId, null, "TopicLecturers").FirstOrDefault();
			if (topic == null)
			{
				throw new BadHttpRequestException("Topic does not exist");
			}
			else
			{
				topicDto = _mapper.Map<GetTopicsDTO>(topic);
				IEnumerable<User> lecturersUserDetails = _unitOfWork.User.GetLecturersByIds(topic.TopicLecturers.Select(x => x.LecturerId).ToArray());
				if (topic.CompanyId != null)
				{
					User companyUser = _unitOfWork.User.Get(x => x.Id == topicDto.CompanyId, null, "Role").FirstOrDefault();
					topicDto.CompanyDto = _mapper.Map<GetCompanyDTO>(companyUser);
				}
				topicDto.LecturerDtos = _mapper.Map<IEnumerable<GetLecturerDTO>>(lecturersUserDetails);
			}
			return topicDto;
		}
	}
}
