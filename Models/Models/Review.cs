using System;
using System.Collections.Generic;

#nullable disable

namespace Models.Models
{
    public partial class Review
    {
        public Review()
        {
            ReviewGrades = new HashSet<ReviewGrade>();
            ReviewQuestions = new HashSet<ReviewQuestion>();
        }

        public Guid Id { get; set; }
        public Guid CouncilId { get; set; }
        public Guid ProjectId { get; set; }
        public bool IsFinal { get; set; }
        public long Date { get; set; }

        public virtual Council Council { get; set; }
        public virtual Project Project { get; set; }
        public virtual ICollection<ReviewGrade> ReviewGrades { get; set; }
        public virtual ICollection<ReviewQuestion> ReviewQuestions { get; set; }
    }
}
