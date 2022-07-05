using System;
using System.Collections.Generic;

namespace Models.Response
{
	public class GetTeamDetailResponse : GetTeamResponse
	{
        public IList<GetLecturerResponse> Mentors { get; set; }
		public IList<Member> Members { get; set; }
	}

}
