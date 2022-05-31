using System;
using System.Collections.Generic;

#nullable disable

namespace Models.Models
{
    public partial class QuestionCopy
    {
        public QuestionCopy()
        {
            ReviewQuestions = new HashSet<ReviewQuestion>();
        }

        public Guid Id { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public Guid CriteriaId { get; set; }

        public virtual SemesterCriterion Criteria { get; set; }
        public virtual ICollection<ReviewQuestion> ReviewQuestions { get; set; }
    }
}
