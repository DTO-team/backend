using System;
using System.Collections.Generic;

namespace Models.Response
{
	public class GetWeeklyReportDetailResponse
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
		public IList<FeedbackResponse> Feedbacks { get; set; }
		public GetWeekResponse Week { get; set; }
		public IEnumerable<GetTeamWeeklyReportsEvidenceResponse> ReportsEvidences { get; set; }
    }

	public class FeedbackResponse
	{
		public Guid Id { get; set; }
		public string Content { get; set; }
		public GetLecturerResponse Author { get; set; }
		public long CreatedDateTime { get; set; }
	}
}