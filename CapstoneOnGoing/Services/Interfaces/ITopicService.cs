using System.Collections.Generic;
using Models.Request;

namespace CapstoneOnGoing.Services.Interfaces
{
	public interface ITopicService
	{
		bool ImportTopics(IEnumerable<ImportTopicsRequest> importTopicsRequest);
	}
}
