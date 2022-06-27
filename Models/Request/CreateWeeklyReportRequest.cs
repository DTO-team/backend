using System;
using System.Collections.Generic;

namespace Models.Request
{
	public class CreateWeeklyReportRequest
	{
		public Guid ProjectId { get; set; }
		public bool IsTeamReport { get; set; } = false;
		public string CompletedTasks { get; set; }
		public string InProgressTasks { get; set; }
		public string NextWeekTasks { get; set; }
		public string UrgentIssues { get; set; }
		public string SelfAssessment { get; set; }
		public string FeedBack { get; set; }
		public IEnumerable<ReportEvidenceRequest> ReportEvidences { get; set; }
	}

	public class ReportEvidenceRequest
	{
		public string Name { get; set; }
		public string Url { get; set; }
	}
}