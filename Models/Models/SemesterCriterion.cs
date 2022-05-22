using System;
using System.Collections.Generic;

#nullable disable

namespace Models.Models
{
    public partial class SemesterCriterion
    {
        public SemesterCriterion()
        {
            EvaluationSessionCriteria = new HashSet<EvaluationSessionCriterion>();
            GradeCopies = new HashSet<GradeCopy>();
            QuestionCopies = new HashSet<QuestionCopy>();
        }

        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Evaluation { get; set; }
        public Guid SemesterId { get; set; }

        public virtual Semester Semester { get; set; }
        public virtual ICollection<EvaluationSessionCriterion> EvaluationSessionCriteria { get; set; }
        public virtual ICollection<GradeCopy> GradeCopies { get; set; }
        public virtual ICollection<QuestionCopy> QuestionCopies { get; set; }
    }
}
