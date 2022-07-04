using System;
using System.Collections.Generic;

#nullable disable

namespace Models.Models
{
    public partial class Feedback
    {
        public Guid Id { get; set; }
        public string Content { get; set; }
        public Guid AuthorId { get; set; }
        public long CreatedDateTime { get; set; }
        public Guid ReportId { get; set; }

        public virtual Lecturer Author { get; set; }
        public virtual Report Report { get; set; }
    }
}
