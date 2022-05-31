using System;
using System.Collections.Generic;

#nullable disable

namespace Models.Models
{
    public partial class Grade
    {
        public Guid Id { get; set; }
        public Guid CriteriaId { get; set; }
        public string Level { get; set; }
        public int MinPoint { get; set; }
        public int MaxPoint { get; set; }
        public string Description { get; set; }

        public virtual Criterion Criteria { get; set; }
    }
}
