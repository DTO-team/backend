using System.Collections.Generic;
using Models.Dtos;
using Models.Request;
using Models.Response;

namespace CapstoneOnGoing.Services.Interfaces
{
	public interface ITopicService
	{
		bool ImportTopics(IEnumerable<ImportTopicsRequest> importTopicsRequest);
		IEnumerable<GetTopicsDTO> GetAllTopics();
	}
}
