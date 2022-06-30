using System;
using System.Collections.Generic;

namespace Models.Response
{
	public class GetTeamDetailResponse : GetTeamResponse
	{
        public Guid ProjectId { get; set; }
        public bool IsApplicationApproved { get; set; }
		public IList<GetLecturerResponse> Mentors { get; set; }
		public IList<Member> Members { get; set; }
	}

}
