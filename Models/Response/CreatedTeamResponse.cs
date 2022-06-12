using System;
using Models.Models;

namespace Models.Response
{
	public class CreatedTeamResponse
	{
		public Guid TeamId { get; set; }
		public string Name { get; set; }
		public string TeamLeaderName { get; set; }
		public string TeamLeaderEmail { get; set; }
	}
}
