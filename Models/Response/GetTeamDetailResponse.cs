using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Response
{
	public class GetTeamDetailResponse : GetTeamResponse
	{
		public IList<GetLecturerResponse> Mentors { get; set; }
		public IList<Member> Members { get; set; }
	}

}
