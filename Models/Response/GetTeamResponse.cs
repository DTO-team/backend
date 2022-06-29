using System;
using System.Collections.Generic;

namespace Models.Response
{
	public class GetTeamResponse
	{
        public bool isApplicationApprove { get; set; }
		public Guid TeamId { get; set; }
		public string TeamName { get; set; }
		public Member Leader { get; set; }
		public int TotalMember { get; set; }

		public GetTeamResponse()
		{
		}
	}

	public class Member
	{
		public Guid Id { get; set; }
		public string Code { get; set; }
		public string FullName { get; set; }
		public string Email { get; set; }
		public string Role { get; set; }
		public string Semester { get; set; }
		public UserStatusResponse Status { get; set; }
		public string AvatarUrl { get; set; }
	}
}
