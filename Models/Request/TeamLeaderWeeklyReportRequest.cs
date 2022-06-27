using System;
using System.ComponentModel.DataAnnotations;

namespace Models.Request
{
    public class TeamLeaderWeeklyReportRequest
    {
        [Required]
        public Guid ProjectId { get; set; }
        [Required]
        public Guid ReporterId { get; set; }
        public Guid WeekId { get; set; }
        public bool IsTeamReport { get; set; }
        public long Deadline { get; set; }
        public string CompletedTasks { get; set; }
        public string InProgressTasks { get; set; }
        public string NextWeekTasks { get; set; }
        public string UrgentIssues { get; set; }
        public string SelfAssessments { get; set; }
        public string Feedback { get; set; }
    }
}
