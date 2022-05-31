using System;
using System.Collections.Generic;

#nullable disable

namespace Models.Models
{
    public partial class EvaluationSession
    {
        public EvaluationSession()
        {
            Councils = new HashSet<Council>();
            EvaluationSessionCriteria = new HashSet<EvaluationSessionCriterion>();
        }

        public Guid Id { get; set; }
        public Guid SemesterId { get; set; }
        public int Round { get; set; }
        public bool IsFinal { get; set; }

        public virtual Semester Semester { get; set; }
        public virtual ICollection<Council> Councils { get; set; }
        public virtual ICollection<EvaluationSessionCriterion> EvaluationSessionCriteria { get; set; }
    }
}
