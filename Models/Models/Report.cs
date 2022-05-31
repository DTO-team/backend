using System;
using System.Collections.Generic;

#nullable disable

namespace Models.Models
{
    public partial class Report
    {
        public Report()
        {
            Evidences = new HashSet<Evidence>();
        }

        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public bool IsTeamReport { get; set; }
        public Guid ReporterId { get; set; }
        public int Week { get; set; }
        public long FromDate { get; set; }
        public long ToDate { get; set; }
        public long Deadline { get; set; }
        public string CompletedTasks { get; set; }
        public string InProgressTasks { get; set; }
        public string NextWeekTasks { get; set; }
        public string UrgentIssues { get; set; }
        public string SelfAssessments { get; set; }
        public string Feedbacks { get; set; }

        public virtual Project Project { get; set; }
        public virtual Student Reporter { get; set; }
        public virtual ICollection<Evidence> Evidences { get; set; }
    }
}
