using System;
using System.Collections.Generic;

namespace Models.Dtos
{
	public class GetTopicsDTO
	{
		public Guid TopicId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public IEnumerable<Guid> LecturerIds { get; set; }
		public IEnumerable<GetLecturerDTO> LecturerDtos { get; set; }
		public Guid CompanyId { get; set; }
		public GetCompanyDTO CompanyDto { get; set; }
	}
}
