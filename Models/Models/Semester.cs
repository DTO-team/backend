using System;
using System.Collections.Generic;

#nullable disable

namespace Models.Models
{
    public partial class Semester
    {
        public Semester()
        {
            EvaluationSessions = new HashSet<EvaluationSession>();
            Students = new HashSet<Student>();
            Teams = new HashSet<Team>();
            Topics = new HashSet<Topic>();
            Weeks = new HashSet<Week>();
        }

        public Guid Id { get; set; }
        public int Year { get; set; }
        public string Season { get; set; }
        public int Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public virtual SemesterCriterion SemesterCriterion { get; set; }
        public virtual ICollection<EvaluationSession> EvaluationSessions { get; set; }
        public virtual ICollection<Student> Students { get; set; }
        public virtual ICollection<Team> Teams { get; set; }
        public virtual ICollection<Topic> Topics { get; set; }
        public virtual ICollection<Week> Weeks { get; set; }
    }
}
