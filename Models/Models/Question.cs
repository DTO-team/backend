using System;
using System.Collections.Generic;

#nullable disable

namespace Models.Models
{
    public partial class Question
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public string Priority { get; set; }
        public Guid CriteriaId { get; set; }

        public virtual Criterion Criteria { get; set; }
    }
}
