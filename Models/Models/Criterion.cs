using System;
using System.Collections.Generic;

#nullable disable

namespace Models.Models
{
    public partial class Criterion
    {
        public Criterion()
        {
            Grades = new HashSet<Grade>();
            Questions = new HashSet<Question>();
        }

        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Evaluation { get; set; }

        public virtual ICollection<Grade> Grades { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
    }
}
