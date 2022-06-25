using System;
namespace Models.Dtos
{
	public class CreateWeeklyReportDTO
	{
		public Guid TeamId { get; set; }
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
		public string ReportEvidenceName { get; set; }
		public string ReportEvidenceUrl { get; set; }
	}
}
