using System;
using System.Collections.Generic;

namespace Models.Response
{
	public class GetWeeklyReportDetailResponse
	{
		public Guid Id { get; set; }
		public Guid ProjectId { get; set; }
		public bool IsTeamReport { get; set; }
		public Reporter ReporterId { get; set; }
		public string CompletedTasks { get; set; }
		public string InProgressTasks { get; set; }
		public string NextWeekTasks { get; set; }
		public string UrgentIssues { get; set; }
		public string SelfAssessments { get; set; }
		public string Feedbacks { get; set; }
		public GetWeekResponse Week { get; set; }
		public IEnumerable<GetTeamWeeklyReportsEvidenceResponse> ReportsEvidences { get; set; }
    }

	public class Reporter
	{
		public Guid Id { get; set; }
		public Guid TeamId { get; set; }
		public string Email { get; set; }
		public string UserName { get; set; }
		public string FullName { get; set; }
		public string Code { get; set; }
		public string Semester { get; set; }
		public string Role { get; set; }
		public UserStatusResponse Status { get; set; }
	}
}