using System;

namespace Models.Request
{
	public class CreateWeeklyReportRequest
	{
		public bool IsTeamReport { get; set; }
		public long FromDate { get; set; }
		public long ToDate { get; set; }
		public long Deadline { get; set; }
		public string CompletedTasks { get; set; }
		public string InProgressTasks { get; set; }
		public string NextWeekTasks { get; set; }
		public string UrgentIssues { get; set; }
		public string SelfAssessment { get; set; }
		public string FeedBack { get; set; }
	}

	public class ReportEvidence
	{
		public string Name { get; set; }
		public string Description { get; set; }
		public string Url { get; set; }
	}
}