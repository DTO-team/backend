using System;
using System.Collections.Generic;

#nullable disable

namespace Models.Models
{
    public partial class ReviewQuestion
    {
        public Guid Id { get; set; }
        public Guid ReviewId { get; set; }
        public Guid QuestionId { get; set; }
        public string Answer { get; set; }
        public string Note { get; set; }

        public virtual QuestionCopy Question { get; set; }
        public virtual Review Review { get; set; }
    }
}
