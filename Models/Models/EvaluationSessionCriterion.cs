using System;
using System.Collections.Generic;

#nullable disable

namespace Models.Models
{
    public partial class EvaluationSessionCriterion
    {
        public Guid Id { get; set; }
        public Guid EvaluationSessionId { get; set; }
        public Guid CriteriaId { get; set; }

        public virtual SemesterCriterion Criteria { get; set; }
        public virtual EvaluationSession EvaluationSession { get; set; }
    }
}
