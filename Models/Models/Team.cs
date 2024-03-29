﻿using System;
using System.Collections.Generic;

#nullable disable

namespace Models.Models
{
    public partial class Team
    {
        public Team()
        {
            Applications = new HashSet<Application>();
            EvaluationReports = new HashSet<EvaluationReport>();
            TeamStudents = new HashSet<TeamStudent>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Status { get; set; }
        public Guid? SemesterId { get; set; }
        public string JoinCode { get; set; }
        public Guid TeamLeaderId { get; set; }

        public virtual Semester Semester { get; set; }
        public virtual Student TeamLeader { get; set; }
        public virtual Project Project { get; set; }
        public virtual ICollection<Application> Applications { get; set; }
        public virtual ICollection<EvaluationReport> EvaluationReports { get; set; }
        public virtual ICollection<TeamStudent> TeamStudents { get; set; }
    }
}
