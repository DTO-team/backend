using System;

namespace Models.Response
{
	public class GetTeamWeeklyReport
	{
		public Guid Id { get; set; }
		public Guid ProjectId { get; set; }
		public bool IsTeamReport { get; set; }
		public Guid ReporterId { get; set; }
		public string CompletedTasks { get; set; }
		public string InProgressTasks { get; set; }
		public string NextWeekTasks { get; set; }
		public string UrgentIssues { get; set; }
		public string SelfAssessments { get; set; }
		public string Feedbacks { get; set; }
        public GetWeekResponse Week { get; set; }
	}
}