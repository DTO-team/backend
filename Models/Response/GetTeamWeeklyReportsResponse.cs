using System;

namespace Models.Response
{
	public class GetTeamWeeklyReportsResponse
	{
		public Guid Id { get; set; }
		public Guid ProjectId { get; set; }
		public bool IsTeamReport { get; set; }
		public string CompletedTasks { get; set; }
	}

	public class GetTeamWeeklyReportsEvidenceResponse
	{

	}
}