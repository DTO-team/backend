using System;
using System.Collections.Generic;
using System.Linq;
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
		public TopicService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public bool ImportTopics(IEnumerable<ImportTopicsRequest> importTopicsRequest)
		{
			bool isSuccessful = true;
			//Get lecture
			foreach (ImportTopicsRequest importTopic in importTopicsRequest)
			{
				User lecturer =
					_unitOfWork.User.Get(x => (importTopic.LecturerEmail.Equals(x.Email) && x.RoleId == 2)).FirstOrDefault();
				if (lecturer != null)
				{
					_unitOfWork.Topic.Insert(new Topic{});
				}
				else
				{
					throw new BadHttpRequestException($"Lecturer with {importTopic.LecturerEmail} does not exist");
				}
			}
			return isSuccessful;
		}
	}
}
