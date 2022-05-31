using System;
using System.Collections.Generic;

#nullable disable

namespace Models.Models
{
    public partial class ReviewGrade
    {
        public Guid Id { get; set; }
        public Guid ReviewId { get; set; }
        public Guid GradeId { get; set; }
        public string Comment { get; set; }

        public virtual GradeCopy Grade { get; set; }
        public virtual Review Review { get; set; }
    }
}
