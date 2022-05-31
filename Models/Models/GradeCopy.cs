using System;
using System.Collections.Generic;

#nullable disable

namespace Models.Models
{
    public partial class GradeCopy
    {
        public GradeCopy()
        {
            ReviewGrades = new HashSet<ReviewGrade>();
        }

        public Guid Id { get; set; }
        public Guid CriteriaId { get; set; }
        public string Level { get; set; }
        public int MinPoint { get; set; }
        public int MaxPoint { get; set; }
        public string Description { get; set; }

        public virtual SemesterCriterion Criteria { get; set; }
        public virtual ICollection<ReviewGrade> ReviewGrades { get; set; }
    }
}
