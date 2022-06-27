using System;
using System.Collections.Generic;

namespace Models.Dtos
{
	public class CreateWeeklyReportDTO
	{
		public Guid ProjectId { get; set; }
		public bool IsTeamReport { get; set; }
		public string CompletedTasks { get; set; }
		public string InProgressTasks { get; set; }
		public string NextWeekTasks { get; set; }
		public string UrgentIssues { get; set; }
		public string SelfAssessment { get; set; }
		public string FeedBack { get; set; }
		public IEnumerable<ReportEvidenceDTO> ReportEvidences { get; set; }
	}
}
