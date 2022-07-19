using System;
using System.Collections.Generic;

namespace Models.Request
{
	public class UpdateCouncilRequest
	{
		public IEnumerable<Guid> LecturerIds { get; set; }
		public IEnumerable<Guid> ProjectIds { get; set; }
	}
}