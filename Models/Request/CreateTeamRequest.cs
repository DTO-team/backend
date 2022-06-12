using System;

namespace Models.Request
{
	public class CreateTeamRequest
	{
		public Guid StudentId { get; set; }
		public string TeamName { get; set; }
	}
}
