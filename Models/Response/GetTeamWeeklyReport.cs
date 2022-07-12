using System;
using System.Collections.Generic;

namespace Models.Response
{
	public class GetTeamWeeklyReportResponse
	{
		public Guid Id { get; set; }
		public Guid ProjectId { get; set; }
		public bool IsTeamReport { get; set; }
		public StudentResponse Reporter { get; set; }
		public string CompletedTasks { get; set; }
		public string InProgressTasks { get; set; }
		public string NextWeekTasks { get; set; }
		public string UrgentIssues { get; set; }
		public string SelfAssessments { get; set; }
		public IEnumerable<GetFeedbackResponse> Feedback { get; set; }
        public GetWeekResponse Week { get; set; }
        public IEnumerable<GetTeamWeeklyReportsEvidenceResponse> ReportEvidences { get; set; }
	}
	public class GetTeamWeeklyReportsEvidenceResponse
	{
		public Guid Id { get; set; }
		public string Url { get; set; }
		public string Name { get; set; }
		public Guid ReportId { get; set; }
	}
}