using System;
using System.Collections.Generic;
using CapstoneOnGoing.Filter;
using Models.Dtos;
using Models.Models;
using Models.Request;
using Models.Response;

namespace CapstoneOnGoing.Services.Interfaces
{
	public interface ITopicService
    {
        Topic GetTopicById(Guid id);
		bool ImportTopics(IEnumerable<ImportTopicsRequest> importTopicsRequest);
		IEnumerable<GetTopicsDTO> GetAllTopics(PaginationFilter validFilter,out int totalRecords);
	}
}
